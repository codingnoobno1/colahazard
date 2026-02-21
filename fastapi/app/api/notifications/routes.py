from fastapi import APIRouter

router = APIRouter()


# ─── PUSH TOKEN MANAGEMENT ───────────────────────────────────
@router.post("/register-device")
async def register_device(data: dict):
    return {"registered": True}


@router.delete("/unregister-device/{device_id}")
async def unregister_device(device_id: str):
    return {"unregistered": True}


@router.put("/token")
async def update_push_token(data: dict):
    return {"updated": True}


@router.get("/devices")
async def list_registered_devices():
    return []


# ─── SUBSCRIPTIONS ────────────────────────────────────────────
@router.post("/subscribe")
async def subscribe_to_topic(topic: str = ""):
    return {"subscribed": True}


@router.post("/unsubscribe")
async def unsubscribe_from_topic(topic: str = ""):
    return {"unsubscribed": True}


@router.get("/subscriptions")
async def list_subscriptions():
    return []


# ─── NOTIFICATION FEED ────────────────────────────────────────
@router.get("/")
async def list_notifications(page: int = 1, limit: int = 50, unread_only: bool = False):
    return {"items": [], "total": 0, "unread_count": 0}


@router.get("/{notification_id}")
async def get_notification(notification_id: str):
    return {}


@router.put("/{notification_id}/read")
async def mark_as_read(notification_id: str):
    return {"read": True}


@router.put("/read-all")
async def mark_all_as_read():
    return {"updated_count": 0}


@router.delete("/{notification_id}")
async def delete_notification(notification_id: str):
    return {"deleted": True}


# ─── ALERTS ───────────────────────────────────────────────────
@router.get("/alerts")
async def get_active_alerts():
    return []


@router.post("/alerts")
async def create_alert(data: dict):
    return {"alert_id": "", "message": "Alert created"}


@router.put("/alerts/{alert_id}/dismiss")
async def dismiss_alert(alert_id: str):
    return {"dismissed": True}


@router.get("/alerts/history")
async def alert_history(days: int = 30):
    return []


# ─── PREFERENCES ──────────────────────────────────────────────
@router.get("/preferences")
async def get_notification_preferences():
    return {}


@router.put("/preferences")
async def update_notification_preferences(data: dict):
    return {"updated": True}
