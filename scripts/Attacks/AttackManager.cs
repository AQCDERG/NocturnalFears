using System;
using System.Collections.Generic;
using Godot;
using JayStation.components._3d;

namespace JayStation.scripts.Attacks;

public partial class AttackManager : Node3D {
	[Signal]
	public delegate void AttackChangedEventHandler(AttackAction attack);

	MeleeAttack melee;
	OneshotAttack oneShot;
	OneshotAttack spinRange;
	RapidFire rapidFire;
	AttackAction currentAttack;

	List<AttackAction> attacks;
	public DetectionComponent detectionComponent;
	Timer changeAttackTimer;
	CharacterBody3D Target;


	public override void _Ready() {
		melee = GetNode<MeleeAttack>("%MeleeAttack");
		oneShot = GetNode<OneshotAttack>("%OneshotAttack");
		rapidFire = GetNode<RapidFire>("%RapidFire");
		spinRange = GetNode<OneshotAttack>("%SpinFire");
		attacks = new List<AttackAction> { melee, oneShot, rapidFire, spinRange };
		currentAttack = attacks[0];
		ConfigureTimer();
		detectionComponent = GetNode<DetectionComponent>("%DetectionComponent");
		detectionComponent.ValidBodyEntered += SetTarget;
	}

	public void SwitchAttack() {
		currentAttack.StopAttack();
		Random rnd = new();
		int index = rnd.Next(0, attacks.Count);
		currentAttack = attacks[index];
		currentAttack.currentTarget = Target;
		currentAttack.StartAttack();
	}

	public void ConfigureTimer() {
		changeAttackTimer = new Timer();
		changeAttackTimer.WaitTime = 5;
		changeAttackTimer.Timeout += SwitchAttack;
		AddChild(changeAttackTimer);
		changeAttackTimer.Start();
	}

	public void SetTarget(CharacterBody3D target) {
		Target = target;
	}
}