from google.adk import Agent
from google.adk.models.lite_llm import LiteLlm
from google.adk.tools.function_tool import FunctionTool
from google.adk.tools.tool_context import ToolContext

INVOICES = [
        {"id": "INV001", "customer_id": "CUST001", "amount": 1000, "status": "paid"},
        {"id": "INV002", "customer_id": "CUST001", "amount": 2000, "status": "pending"},
        {"id": "INV003", "customer_id": "CUST002", "amount": 3000, "status": "overdue"}
]

def list_invoices(customer_id: str) -> dict:
    """List all invoices for a given customer."""
    return { "invoices": [invoice for invoice in INVOICES if invoice["customer_id"] == customer_id]}

# def cancell_signature(invoice_id: str, tool_context: ToolContext) -> dict:
def cancell_signature(invoice_id: str) -> dict:
    """Cancel the signature of an invoice."""

    # if tool_context.tool_confirmation is None:
    #     tool_context.request_confirmation(
    #       hint="Signature value is too high. Do you want to proceed with cancellation?",
    #         payload={
    #             invoice_id: invoice_id,
    #             "password": ""
    #         }
    #     )
    #     return {"status": "confirmation_requested"}
    
    # if tool_context.tool_confirmation.confirmed:
    #     return {"status": "cancellation_confirmed", "message": f"Cancellation of invoice {invoice_id} approved."}

    for invoice in INVOICES:
        if invoice["id"] == invoice_id:
            if invoice["status"] == "paid":
                return {"error": "Cannot cancel signature of a paid invoice."}
            invoice["status"] = "cancelled"
            return {"message": f"Invoice {invoice_id} has been cancelled."}
    return {"error": "Invoice not found."}

def verify_signature_value(invoice_id: str) -> bool:
    """Verify the signature value of an invoice."""
    for invoice in INVOICES:
        if invoice["id"] == invoice_id:
            return invoice["amount"] > 2500
    return False

root_agent = Agent(
    name="my_agent",
    instruction="""
        You are a helpful assistant that manages customer invoices. 
        You can list all invoices for a given customer and cancel the signature of an invoice. 
    """,
    model="gemini-2.5-flash-lite",
    # model=LiteLlm("anthropic/claude-sonnet-4-6")
    tools=[
        list_invoices,
        # FunctionTool(cancell_signature, require_confirmation=True),
        FunctionTool(cancell_signature, require_confirmation=verify_signature_value),
    ]
)