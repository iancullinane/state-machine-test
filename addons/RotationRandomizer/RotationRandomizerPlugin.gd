@tool
extends EditorPlugin

var dock

func _enter_tree() -> void:
	dock = preload("res://addons/RotationRandomizer/RotationRandomizer.tscn").instantiate()
	dock.undo_redo = get_undo_redo()
	add_control_to_dock(EditorPlugin.DOCK_SLOT_LEFT_BL, dock)

func _exit_tree() -> void:
	remove_control_from_docks(dock)
	dock.free()
