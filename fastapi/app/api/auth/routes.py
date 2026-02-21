from fastapi import APIRouter, Depends, HTTPException, status
from fastapi.security import OAuth2PasswordRequestForm

router = APIRouter()


# ─── LOGIN / LOGOUT ───────────────────────────────────────────
@router.post("/login")
async def login(form: OAuth2PasswordRequestForm = Depends()):
    return {"access_token": "", "refresh_token": "", "token_type": "bearer"}


@router.post("/refresh")
async def refresh_token(refresh_token: str = ""):
    return {"access_token": "", "token_type": "bearer"}


@router.post("/logout")
async def logout():
    return {"message": "Logged out"}


# ─── REGISTRATION ─────────────────────────────────────────────
@router.post("/register")
async def register(data: dict):
    return {"message": "User registered", "user_id": ""}


@router.post("/verify-otp")
async def verify_otp(otp: str = "", phone: str = ""):
    return {"verified": True}


@router.post("/resend-otp")
async def resend_otp(phone: str = ""):
    return {"message": "OTP sent"}


# ─── PASSWORD MANAGEMENT ──────────────────────────────────────
@router.post("/forgot-password")
async def forgot_password(email: str = ""):
    return {"message": "Reset link sent"}


@router.post("/reset-password")
async def reset_password(token: str = "", new_password: str = ""):
    return {"message": "Password reset successful"}


@router.post("/change-password")
async def change_password(old_password: str = "", new_password: str = ""):
    return {"message": "Password changed"}


# ─── ROLES & PERMISSIONS ──────────────────────────────────────
@router.get("/roles")
async def list_roles():
    return []


@router.get("/roles/{role_id}/permissions")
async def get_role_permissions(role_id: str):
    return []


@router.post("/roles")
async def create_role(data: dict):
    return {"role_id": "", "message": "Role created"}


@router.put("/roles/{role_id}")
async def update_role(role_id: str, data: dict):
    return {"message": "Role updated"}


@router.delete("/roles/{role_id}")
async def delete_role(role_id: str):
    return {"message": "Role deleted"}


# ─── PROFILE ──────────────────────────────────────────────────
@router.get("/me")
async def get_profile():
    return {}


@router.put("/me")
async def update_profile(data: dict):
    return {"message": "Profile updated"}
