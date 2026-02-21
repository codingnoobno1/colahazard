from fastapi import APIRouter

from app.api.auth.routes import router as auth_router
from app.api.mobile.qr.routes import router as qr_router
from app.api.mobile.sync.routes import router as sync_router
from app.api.notifications.routes import router as notifications_router

from app.api.mobile.dashboard.transport.shipments import router as transport_shipments
from app.api.mobile.dashboard.factory.batches import router as factory_batches
from app.api.mobile.dashboard.retailer.inventory import router as retailer_inventory
from app.api.mobile.dashboard.wholesaler.orders import router as wholesaler_orders

api_router = APIRouter()

api_router.include_router(auth_router, prefix="/auth", tags=["Auth"])
api_router.include_router(qr_router, prefix="/qr", tags=["QR"])
api_router.include_router(sync_router, prefix="/sync", tags=["Sync"])
api_router.include_router(notifications_router, prefix="/notifications", tags=["Notifications"])

api_router.include_router(transport_shipments, prefix="/dashboard/transport", tags=["Transport"])
api_router.include_router(factory_batches, prefix="/dashboard/factory", tags=["Factory"])
api_router.include_router(retailer_inventory, prefix="/dashboard/retailer", tags=["Retailer"])
api_router.include_router(wholesaler_orders, prefix="/dashboard/wholesaler", tags=["Wholesaler"])
