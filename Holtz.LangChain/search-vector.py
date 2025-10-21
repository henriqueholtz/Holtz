import os
import sys
from dotenv import load_dotenv

from langchain_google_genai import GoogleGenerativeAIEmbeddings
from langchain_postgres import PGVector

load_dotenv()
for k in ("GOOGLE_API_KEY", "GOOGLE_EMBEDDING_MODEL", "PGVECTOR_URL","PGVECTOR_COLLECTION"):
    if not os.getenv(k):
        raise RuntimeError(f"Environment variable {k} is not set")
        
if len(sys.argv) < 2:
    print("Usage: python search-vector.py \"<query>\"")
    print("Example: python search-vector.py \"Tell me briefly about cassandra structure\"")
    sys.exit(1)

query = sys.argv[1]

embeddings = GoogleGenerativeAIEmbeddings(model=os.getenv("GOOGLE_EMBEDDING_MODEL","gemini-embedding-001"))

store = PGVector(
    embeddings=embeddings,
    collection_name=os.getenv("PGVECTOR_COLLECTION"),
    connection=os.getenv("PGVECTOR_URL"),
    use_jsonb=True,
)

results = store.similarity_search_with_score(query, k=3)

for i, (doc, score) in enumerate(results, start=1):
    print("-"*80)
    print(f"Resultado {i} (score: {score:.2f}):")
    print("\nTexto:\n")
    print(doc.page_content.strip())

    print("\nMetadados:\n")
    for k, v in doc.metadata.items():
        print(f"{k}: {v}")

