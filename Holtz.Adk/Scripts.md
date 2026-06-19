# Scripts

- Required python 3.12

```python
uv init .

uv run main.py

source .venv/Scripts/activate # OR similar as "source ./.venv/bin/activate.fish"

uv add google-adk===2.2.0
adk --version # Will work if the venv is active
adk run agents/my_agent

uv add litellm>=1.75.5
```
