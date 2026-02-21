from fastapi import APIRouter

router = APIRouter()


# ─── PUSH (OFFLINE → SERVER) ─────────────────────────────────
@router.post("/push")
async def push_offline_changes(data: dict):
    return {"accepted": 0, "rejected": 0, "conflicts": []}


@router.post("/push/scans")
async def push_offline_scans(data: dict):
    return {"synced": 0}


@router.post("/push/transactions")
async def push_offline_transactions(data: dict):
    return {"synced": 0}


@router.post("/push/inspections")
async def push_offline_inspections(data: dict):
    return {"synced": 0}


@router.post("/push/signatures")
async def push_offline_signatures(data: dict):
    return {"synced": 0}


@router.post("/push/photos")
async def push_offline_photos(data: dict):
    return {"synced": 0}


# ─── PULL (SERVER → DEVICE) ──────────────────────────────────
@router.get("/pull")
async def pull_delta(last_sync: str = ""):
    return {"changes": [], "timestamp": ""}


@router.get("/pull/products")
async def pull_products(last_sync: str = ""):
    return {"products": [], "timestamp": ""}


@router.get("/pull/inventory")
async def pull_inventory(last_sync: str = ""):
    return {"inventory": [], "timestamp": ""}


@router.get("/pull/shipments")
async def pull_shipments(last_sync: str = ""):
    return {"shipments": [], "timestamp": ""}


@router.get("/pull/routes")
async def pull_routes(last_sync: str = ""):
    return {"routes": [], "timestamp": ""}


@router.get("/pull/config")
async def pull_config(last_sync: str = ""):
    return {"config": {}, "timestamp": ""}


@router.get("/pull/notifications")
async def pull_notifications(last_sync: str = ""):
    return {"notifications": [], "timestamp": ""}


# ─── CONFLICT RESOLUTION ─────────────────────────────────────
@router.get("/conflicts")
async def list_conflicts():
    return []


@router.post("/conflicts/{conflict_id}/resolve")
async def resolve_conflict(conflict_id: str, resolution: str = "server"):
    return {"resolved": True}


@router.post("/conflicts/resolve-all")
async def resolve_all_conflicts(strategy: str = "server_wins"):
    return {"resolved_count": 0}


@router.get("/conflicts/{conflict_id}")
async def get_conflict_details(conflict_id: str):
    return {}


# ─── SYNC STATUS ──────────────────────────────────────────────
@router.get("/status")
async def sync_status():
    return {"last_sync": "", "pending_changes": 0, "status": "idle"}


@router.get("/timestamp")
async def last_sync_timestamp():
    return {"timestamp": "", "server_time": ""}


@router.post("/force")
async def force_full_sync():
    return {"status": "started", "job_id": ""}


@router.get("/queue")
async def get_sync_queue():
    return {"pending": [], "failed": []}


@router.delete("/queue/{item_id}")
async def remove_from_queue(item_id: str):
    return {"removed": True}


# ─── SYNC HEALTH ──────────────────────────────────────────────
@router.get("/health")
async def sync_health():
    return {"connectivity": True, "latency_ms": 0, "queue_size": 0}


@router.get("/diagnostics")
async def sync_diagnostics():
    return {"last_errors": [], "stats": {}}


@router.post("/reset")
async def reset_sync_state():
    return {"message": "Sync state reset"}


@router.get("/bandwidth")
async def bandwidth_estimate():
    return {"estimated_mb": 0, "wifi_recommended": False}
