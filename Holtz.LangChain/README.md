# Holtz.LangChain

https://github.com/devfullcycle/mba-ia-niv-introducao-langchain

### Commands

Creating and activating the venc and installing most dependencies

```
python3 -m venv venv # create a virtual env called venv

source venv/Scripts/activate # activate the virtual env

pip install langchain langchain-openai langchain-google-genai python-dotenv beautifulsoup4 pypdf langchain_community

pip freeze > requirements.txt
```

Install new dependencies

```
pip install package_name

pip freeze > requirements.txt
```

## Requirements

- python
- Docker

## Run and test

- Create and activate your venv (virtual env) as mentioned above
- `docker compose up -d`
- `python ingestion-pgvector.py Kafka.pdf`
- `python ingestion-pgvector.py cassandra.pdf`
- `python search-vector.py "Tell me briefly about cassandra structure"`
