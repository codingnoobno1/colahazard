using Microsoft.AspNetCore.Components;

namespace PackTrack.Components;

/// <summary>
/// Enterprise-grade base component that prevents ObjectDisposedException
/// in Blazor Server circuits. All components with async updates, timers,
/// or background tasks should inherit from this instead of ComponentBase.
/// </summary>
public abstract class SafeComponent : ComponentBase, IDisposable
{
    protected bool Disposed { get; private set; }

    /// <summary>
    /// Thread-safe refresh that guards against disposed render trees.
    /// Use this instead of calling StateHasChanged() directly.
    /// </summary>
    protected async Task SafeRefresh()
    {
        if (!Disposed)
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Synchronous variant — only call from the Blazor sync context.
    /// </summary>
    protected void SafeStateHasChanged()
    {
        if (!Disposed) StateHasChanged();
    }

    public virtual void Dispose()
    {
        Disposed = true;
    }
}
