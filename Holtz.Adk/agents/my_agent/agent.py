from google.adk import Agent
from google.adk.models.lite_llm import LiteLlm
from google.adk.tools.function_tool import FunctionTool

INVOICES = [
        {"id": "INV001", "customer_id": "CUST001", "amount": 1000, "status": "paid"},
        {"id": "INV002", "customer_id": "CUST001", "amount": 2000, "status": "pending"},
        {"id": "INV003", "customer_id": "CUST002", "amount": 3000, "status": "overdue"}
]

def list_invoices(customer_id: str) -> dict:
    """List all invoices for a given customer."""
    return { "invoices": [invoice for invoice in INVOICES if invoice["customer_id"] == customer_id]}

def cancell_signature(invoice_id: str) -> dict:
    """Cancel the signature of an invoice."""
    for invoice in INVOICES:
        if invoice["id"] == invoice_id:
            if invoice["status"] == "paid":
                return {"error": "Cannot cancel signature of a paid invoice."}
            invoice["status"] = "cancelled"
            return {"message": f"Invoice {invoice_id} has been cancelled."}
    return {"error": "Invoice not found."}

root_agent = Agent(
    name="my_agent",
    instruction="""
        You are a Python developer
    """,
    model="gemini-2.5-flash-lite",
    # model=LiteLlm("anthropic/claude-sonnet-4-6")
    tools=[
        list_invoices,
        FunctionTool(cancell_signature, require_confirmation=True),
    ]
)