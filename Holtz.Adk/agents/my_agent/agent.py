from google.adk import Agent
from google.adk.models.lite_llm import LiteLlm

root_agent = Agent(
    name="my_agent",
    instruction="""
        You are a Python developer
    """,
    model="gemini-2.5-flash-lite"
    # model=LiteLlm("anthropic/claude-sonnet-4-6")
)