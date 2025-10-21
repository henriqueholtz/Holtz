import os
import sys
from pathlib import Path
from dotenv import load_dotenv

from langchain_community.document_loaders import PyPDFLoader
from langchain_text_splitters import RecursiveCharacterTextSplitter
from langchain_google_genai import GoogleGenerativeAIEmbeddings
from langchain_core.documents import Document
from langchain_postgres import PGVector

# Check if the file name is provided as a command-line argument
if len(sys.argv) < 2:
    print("Usage: python ingestion-pgvector.py <pdf_file_name>")
    print("Example: python ingestion-pgvector.py Kafka.pdf")
    sys.exit(1)

pdf_file_name = sys.argv[1]

load_dotenv()
for k in ("GOOGLE_API_KEY", "GOOGLE_EMBEDDING_MODEL", "PGVECTOR_URL","PGVECTOR_COLLECTION"):
    if not os.getenv(k):
        raise RuntimeError(f"Environment variable {k} is not set")

current_dir = Path(__file__).parent
pdf_path = current_dir / pdf_file_name

print(f"Loading PDF from: {pdf_path}")
docs = PyPDFLoader(str(pdf_path)).load()
print(f"Loaded {len(docs)} pages from PDF")

print("Splitting documents into chunks...")
splits = RecursiveCharacterTextSplitter(
    chunk_size=1000, 
    chunk_overlap=150, add_start_index=False).split_documents(docs)
if not splits:
    raise SystemExit(0)

enrichedDocs = [
    Document(
        page_content=d.page_content,
        metadata={k: v for k, v in d.metadata.items() if v not in ("", None)}
    )
    for d in splits
]

ids = [f"doc-{i}" for i in range(len(enrichedDocs))]

embeddings = GoogleGenerativeAIEmbeddings(model=os.getenv("GOOGLE_EMBEDDING_MODEL","gemini-embedding-001"))

store = PGVector(
    embeddings=embeddings,
    collection_name=os.getenv("PGVECTOR_COLLECTION"),
    connection=os.getenv("PGVECTOR_URL"),
    use_jsonb=True,
)

print(f"Adding {len(enrichedDocs)} documents to the vector store...")
store.add_documents(documents=enrichedDocs, ids=ids)
print("Documents successfully added to the vector store!")
