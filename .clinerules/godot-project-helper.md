# Cline Rules — Godot 4 + C#

## Scope & Non-Goals
- **Always** target **Godot 4.x**. Use 4.x classes, APIs, names, and patterns.
- **Never** reference or propose **Godot 3** classes, methods, signals, or style.
- Language is **C#** only (no GDScript in generated code unless explicitly asked).

---

## Coding Standards
- Use **.NET/C# conventions**:
  - `PascalCase` for types, methods, properties; `camelCase` for locals/fields; `_camelCase` for private fields.
  - File-scoped namespaces, nullable enabled.
  - Avoid magic numbers/strings; prefer `const`, `readonly`, or exported properties.
- Prefer **explicit access modifiers**; keep fields `private` unless needed.
- Keep methods short; one responsibility. Refactor early.
- Comments explain *why*, not *what*. Use XML doc comments for public APIs.

**Template**
```csharp
#nullable enable
using Godot;

namespace Game;

public sealed partial class PlayerController : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 240f;
    [Export] public NodePath CameraPath { get; set; } = default!;
    private Camera2D? _camera;

    public override void _Ready()
    {
        // Accessing nodes is an important task
        _camera = GetNodeOrNull<Camera2D>(CameraPath);
        GD.Assert(_camera is not null, "Camera2D is required.");
    }

    public override void _PhysicsProcess(double delta)
    {
        var input = GetInputVector();
        Velocity = input * Speed;
        MoveAndSlide();
    }

    private static Vector2 GetInputVector() =>
        new(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            Input.GetActionStrength("ui_down")  - Input.GetActionStrength("ui_up")
        ).Normalized();
}
