@tool
extends Node

@onready var rotate_x: CheckBox = $VBoxContainer/RotateX
@onready var rotate_y: CheckBox = $VBoxContainer/RotateY
@onready var rotate_z: CheckBox = $VBoxContainer/RotateZ
@onready var random_button: Button = $VBoxContainer/HBoxContainer/RandomButton
@onready var reset_button: Button = $VBoxContainer/HBoxContainer/ResetButton

var undo_redo: EditorUndoRedoManager

func _randomize_rotation() -> void:
	undo_redo.create_action("Randomize rotation")

	for node in _get_selected_nodes():
		var old_rotation = node.rotation
		if rotate_x.button_pressed:
			node.rotation.x = _get_random_rotation()
		if rotate_y.button_pressed:
			node.rotation.y = _get_random_rotation()
		if rotate_z.button_pressed:
			node.rotation.z = _get_random_rotation()

		undo_redo.add_do_property(node, "global_rotation", node.global_rotation)
		undo_redo.add_undo_property(node, "global_rotation", old_rotation)
	undo_redo.commit_action()

func _reset_rotation() -> void:
	undo_redo.create_action("Reset rotation")
	for node in _get_selected_nodes():
		var old_rotation = node.rotation
		if rotate_x.button_pressed:
			node.rotation.x = 0
		if rotate_y.button_pressed:
			node.rotation.y = 0
		if rotate_z.button_pressed:
			node.rotation.z = 0
		undo_redo.add_do_property(node, "global_rotation", node.global_rotation)
		undo_redo.add_undo_property(node, "global_rotation", old_rotation)
	undo_redo.commit_action()

func _get_selected_nodes() -> Array[Node]:
	var selected_nodes = EditorInterface.get_selection().get_selected_nodes()
	print("selected %s nodes" % selected_nodes.size())
	for node in selected_nodes:
		if node is not Node3D:
			selected_nodes.erase(node)

	return selected_nodes

func _get_random_rotation() -> float:
	return deg_to_rad(randf_range(0, 360))