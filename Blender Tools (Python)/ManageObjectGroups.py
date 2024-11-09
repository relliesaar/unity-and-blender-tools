bl_info = {
	"name": "Manage Object Groups",
	"author": "Relliesaar",
	"version": "1, 0",
	"location": "Object menu -> Group / ungroup objects",
	"description": "This add-on allows to create or delete object groups",
	"blender": (4, 1, 0),
	"category": "Object"
}

import bpy
from bpy.types import Operator

class Utilities:
	
	def GroupObjects():
		empty = None
		selectedObjects = bpy.context.selected_objects
  
		if len(bpy.context.selected_objects) > 0:
      
			for obj in bpy.context.selected_objects:
				if obj.type == "EMPTY":
					empty = obj
					break
					
			if empty is None:
				bpy.ops.object.empty_add()
				empty = bpy.context.active_object
			
			for obj in selectedObjects:
				if obj.type == "EMPTY":
					continue
				obj.parent = empty
				obj.matrix_parent_inverse = empty.matrix_world.inverted()
    
   
	def UngroupObjects():
     
		if bpy.context.active_object.type == "EMPTY":
			bpy.ops.object.select_grouped(type = "CHILDREN_RECURSIVE")

		for obj in bpy.context.selected_objects:
			if obj.type == "EMPTY":
				continue
			
			parentedWorldMatrix = obj.matrix_world.copy()
			obj.parent = None
			obj.matrix_world = parentedWorldMatrix


class GroupObjects(Operator):
	
	bl_idname = "utilities.group_objects"
	bl_label = "Group objects"
	bl_options = {"REGISTER", "UNDO"}
 
	def execute(self, context):
	 
		Utilities.GroupObjects()
		return {"FINISHED"}

class UpgroupObjects(Operator):
    
	bl_idname = "utilities.ungroup_objects"
	bl_label = "Ungroup objects"
	bl_options = {"REGISTER", "UNDO"}
 
	def execute(self, context):
     
		Utilities.UngroupObjects()
		return {"FINISHED"}
 
 
def menu_group_objects(self, context):
	self.layout.operator(GroupObjects.bl_idname)
 
def menu_ungroup_objects(self, context):
    self.layout.operator(UpgroupObjects.bl_idname)
	
 
def register():
	bpy.utils.register_class(GroupObjects)
	bpy.types.VIEW3D_MT_object.append(menu_group_objects)
 
	bpy.utils.register_class(UpgroupObjects)
	bpy.types.VIEW3D_MT_object.append(menu_ungroup_objects)
	
def unregister(): 
	bpy.utils.unregister_class(GroupObjects)
	bpy.types.VIEW3D_MT_object.remove(menu_group_objects)
 
	bpy.utils.unregister_class(UpgroupObjects)
	bpy.types.VIEW3D_MT_object.remove(menu_ungroup_objects)