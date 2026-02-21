from fastapi import APIRouter


def create_crud_routes(router: APIRouter, service, prefix: str):
    """Auto-generate standard CRUD endpoints for any entity.
    Use this for 50+ entities to auto-generate ~200 endpoints."""

    @router.get(f"/{prefix}")
    async def list_items():
        return service.list()

    @router.post(f"/{prefix}")
    async def create_item(data: dict):
        return service.create(data)

    @router.get(f"/{prefix}/{{id}}")
    async def get_item(id: str):
        return service.get(id)

    @router.put(f"/{prefix}/{{id}}")
    async def update_item(id: str, data: dict):
        return service.update(id, data)

    @router.delete(f"/{prefix}/{{id}}")
    async def delete_item(id: str):
        return service.delete(id)
