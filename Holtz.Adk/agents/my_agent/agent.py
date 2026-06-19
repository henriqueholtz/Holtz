from google.adk import Agent

root_agent = Agent(
    name="my_agent",
    instruction="""
        You are a Python developer
    """,
    model="gemini-2.5-flash-lite"
)