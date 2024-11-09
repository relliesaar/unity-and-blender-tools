bl_info = {
	"name": "Manage Colliders",
	"author": "Relliesaar",
	"version": "1, 0",
	"location": "Object menu -> Create damage/player/generic clip",
	"description": "This add-on creates simple mesh with specific name and material, suitable for creating collisions.",
	"blender": (4, 1, 0),
	"category": "Object"
}

import bpy
from bpy.types import Operator

class Collision():
	
	def CreateMesh(meshName):
		bpy.ops.mesh.primitive_cube_add()
		mesh = bpy.context.active_object
		mesh.name = meshName
		
		return mesh
	
	def CreateMat(matName, matColor):
		bpy.ops.outliner.orphans_purge(do_local_ids = True, do_linked_ids = True, do_recursive = True) # delete orphan materials
		
		mat = bpy.data.materials.get(matName)
		
		if mat is None:
			mat = bpy.data.materials.new(matName)
			mat.diffuse_color = matColor
			mat.roughness = 1.0
			mat.metallic = 1.0
			
		return mat

	def CreateCollider(meshes, meshName, matName, matColor):
		mat = Collision.CreateMat(matName, matColor)
  
		if meshes: # if meshes are not empty
			for mesh in meshes:
				if mesh.type == "EMPTY": 
					continue
				mesh.name = meshName
				mesh.data.materials.clear()
				mesh.data.materials.append(mat)
		else:
			mesh = Collision.CreateMesh(meshName)
			mesh.data.materials.append(mat)
   
	def FindColliders(colliderName):
		bpy.ops.object.select_pattern(pattern = f"{colliderName}*", extend = False)	
 


class FindAndConvertClips(Operator):
	bl_idname = "collision.find_convert_clips"
	bl_label = "Find and covert clips"
	bl_options = {"REGISTER", "UNDO"}    
		
	def execute(self, context):
		Collision.FindColliders("PlayerClip")
		Collision.CreateCollider(bpy.context.selected_objects, "PlayerClip", "PlayerClipMat", (1.0, 0.25, 0.0, 0.5))	  
  
		Collision.FindColliders("DamageClip")
		Collision.CreateCollider(bpy.context.selected_objects, "DamageClip", "DamageClipMat", (0.3, 0.0, 1.0, 0.5))
  
		Collision.FindColliders("Clip")
		Collision.CreateCollider(bpy.context.selected_objects, "Clip", "ClipMat", (1.0, 0.0, 0.0, 0.5))	 	

		return {"FINISHED"}


class CreateDamageClip(Operator):
	
	bl_idname = "collision.create_dmg_clip"
	bl_label = "Create damage clip"
	bl_options = {"REGISTER", "UNDO"}    
	
	def execute(self, context):
		Collision.CreateCollider(bpy.context.selected_objects, "DamageClip", "DamageClipMat", (0.3, 0.0, 1.0, 0.5))
		return {"FINISHED"}
	
class CreateClip(Operator):

	bl_idname = "collision.create_clip"
	bl_label = "Create clip"
	bl_options = {"REGISTER", "UNDO"}    
	
	def execute(self, context):
		Collision.CreateCollider(bpy.context.selected_objects, "Clip", "ClipMat", (1.0, 0.0, 0.0, 0.5))	 
		return {"FINISHED"}
	
class CreatePlayerClip(Operator):
	
	bl_idname = "collision.create_plr_clip"
	bl_label = "Create player clip"
	bl_options = {"REGISTER", "UNDO"}    
	
	def execute(self, context):
		Collision.CreateCollider(bpy.context.selected_objects, "PlayerClip", "PlayerClipMat", (1.0, 0.25, 0.0, 0.5))
		return {"FINISHED"}


def menu_find_convert_clips(self, context):
	self.layout.operator(FindAndConvertClips.bl_idname)	 
 
def menu_dmg_clip(self, context):
	self.layout.operator(CreateDamageClip.bl_idname)
	
def menu_clip(self, context):
	self.layout.operator(CreateClip.bl_idname)
	
def menu_plr_clip(self, context):
	self.layout.operator(CreatePlayerClip.bl_idname)
	
 
def register():
	bpy.utils.register_class(FindAndConvertClips)
	bpy.types.VIEW3D_MT_object.append(menu_find_convert_clips) 
		
	bpy.utils.register_class(CreateDamageClip)
	bpy.types.VIEW3D_MT_object.append(menu_dmg_clip)
	
	bpy.utils.register_class(CreateClip)
	bpy.types.VIEW3D_MT_object.append(menu_clip)
	
	bpy.utils.register_class(CreatePlayerClip)
	bpy.types.VIEW3D_MT_object.append(menu_plr_clip)
 
	
def unregister():
	bpy.utils.unregister_class(FindAndConvertClips)
	bpy.types.VIEW3D_MT_object.remove(menu_find_convert_clips) 
    
	bpy.utils.unregister_class(CreateDamageClip)
	bpy.types.VIEW3D_MT_object.remove(menu_dmg_clip)
	
	bpy.utils.unregister_class(CreateClip)
	bpy.types.VIEW3D_MT_object.remove(menu_clip)
	
	bpy.utils.unregister_class(CreatePlayerClip)
	bpy.types.VIEW3D_MT_object.remove(menu_plr_clip)