using Godot;
using System;

public partial class Characters : Node3D
{
	[Export] Texture2D SnareIdleTexture;
	[Export] Texture2D SnareLeftTexture;
	[Export] Texture2D SnareRightTexture;

	[Export] Texture2D TimpaniIdleTexture;
	[Export] Texture2D TimpaniLeftTexture;
	[Export] Texture2D TimpaniRightTexture;

	[Export] Texture2D TambourineIdleTexture;
	[Export] Texture2D TambourineHitTexture;

	MeshInstance3D _snareIdle;
	MeshInstance3D _snareHitLeft;
	MeshInstance3D _snareHitRight;
	MeshInstance3D _tambourineIdle;
	MeshInstance3D _tambourineHit;
	MeshInstance3D _timpaniIdle;
	MeshInstance3D _timpaniHitLeft;
	MeshInstance3D _timpaniHitRight;

	public override void _Ready()
	{
		_snareIdle = GetNode<MeshInstance3D>("SnareIdle");
		_snareHitLeft = GetNode<MeshInstance3D>("SnareHitLeft");
		_snareHitRight = GetNode<MeshInstance3D>("SnareHitRight");
		_tambourineIdle = GetNode<MeshInstance3D>("TambourineIdle");
		_tambourineHit = GetNode<MeshInstance3D>("TambourineHit");
		_timpaniIdle = GetNode<MeshInstance3D>("TimpaniIdle");
		_timpaniHitLeft = GetNode<MeshInstance3D>("TimpaniHitLeft");
		_timpaniHitRight = GetNode<MeshInstance3D>("TimpaniHitRight");
	}

	public void HitTambourine()
	{
		_tambourineHit.Visible = true;
		_tambourineIdle.Visible = false;
	}

	public void IdleTambourine()
	{
		_tambourineHit.Visible = false;
		_tambourineIdle.Visible = true;
	}

	bool _isSnareLeft = false;
	public void HitSnare()
	{
		_snareIdle.Visible = false;
		if (_isSnareLeft)
		{
			_snareHitRight.Visible = true;
			_snareHitLeft.Visible = false;	
		}
		else
		{
			_snareHitRight.Visible = false;
			_snareHitLeft.Visible = true;
		}
		_isSnareLeft = !_isSnareLeft;
	}

	public void IdleSnare()
	{
		_snareHitRight.Visible = false;
		_snareHitLeft.Visible = false;
		_snareIdle.Visible = true;
	}
	bool _isTimpaniLeft = false;
	public void HitTimpani()
	{
		_timpaniIdle.Visible = false;
		if (_isTimpaniLeft)
		{
			_timpaniHitRight.Visible = true;
			_timpaniHitLeft.Visible = false;
		}
		else
		{
			_timpaniHitRight.Visible = false;
			_timpaniHitLeft.Visible = true;
		}
		_isTimpaniLeft = !_isTimpaniLeft;
	}
	public void IdleTimpani()
	{
		_timpaniHitRight.Visible = false;
		_timpaniHitLeft.Visible = false;
		_timpaniIdle.Visible = true;
	}
}
