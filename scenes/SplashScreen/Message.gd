@tool
extends RichTextLabel

@export var message: String = "Loading"
@export var _dots_update_interval_seconds: float = 0.5

var _dots: String = ""

var _last_time_msec: int = Time.get_ticks_msec()


func _process(_delta: float) -> void:
	var current_time_msec: int = Time.get_ticks_msec()
	if _should_run_update(current_time_msec):
		_last_time_msec = current_time_msec
		_update_message()

func _should_run_update(current_time_msec: int) -> bool:
	if (current_time_msec - _last_time_msec) > _to_msec(_dots_update_interval_seconds):
		return true
	return false
	
func _to_msec(seconds: float) -> float:
	return seconds * 1000

func _update_message() -> void:
	_dots += "."
	if _dots.contains("...."):
		_dots = ""

	text = message + _dots;


