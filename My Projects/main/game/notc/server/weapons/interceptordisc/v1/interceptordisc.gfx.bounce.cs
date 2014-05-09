// Copyright information can be found in the file named COPYING
// located in the root directory of this distribution.

datablock ParticleData(WpnInterceptorDiscBounceEffect_DebrisParticles)
{
	spinSpeed = 200;
	spinRandomMin = -200.0;
	spinRandomMax =  200.0;
	dragCoefficient		= 1;
	gravityCoefficient	= 2.5;
	windCoefficient		= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration = 0.0;
	lifetimeMS			  = 1500;
	lifetimeVarianceMS	= 0;
	textureName = "content/xa/rotc/p.5.4/shapes/rotc/misc/debris1.white";
	colors[0]	  = "1.0 1.0 1.0 1.0";
	colors[1]	  = "1.0 1.0 1.0 1.0";
	colors[2]	  = "1.0 1.0 1.0 0.0";
	sizes[0]		= 0.25;
	sizes[1]		= 0.25;
	sizes[2]		= 0.25;
	times[0]		= 0.0;
	times[1]		= 0.5;
	times[2]		= 1.0;
	useInvAlpha =  false;
	allowLighting = false;
};

datablock ParticleEmitterData(WpnInterceptorDiscBounceEffect_DebrisEmitter)
{
	ejectionPeriodMS = 50;
	periodVarianceMS = 0;
	ejectionVelocity = 10.0;
	velocityVariance = 5.0;
	ejectionOffset	= 0.0;
	thetaMin			= 0;
	thetaMax			= 60;
	phiReferenceVel  = 0;
	phiVariance		= 360;
	lifetimeMS		 = 5;
	lifetimeVarianceMS = 0;
	overrideAdvances = false;
	orientParticles  = true;
	particles = "WpnInterceptorDiscBounceEffect_DebrisParticles";
};

datablock ExplosionData(WpnInterceptorDiscBounceEffect)
{
	soundProfile = WpnInterceptorDiscBounceEffectSound;

	lifetimeMS = 300;

	// shape...
	//explosionShape = "content/xa/rotc/p.5.4/shapes/rotc/effects/explosion5.green.dts";
	faceViewer = false;
	playSpeed = 4.0;
	sizes[0] = "0.01 0.01 0.01";
	sizes[1] = "0.20 0.20 0.20";
	times[0] = 0.0;
	times[1] = 1.0;

	emitter[0] = WpnInterceptorDiscBounceEffect_DebrisEmitter;

	//debris = WpnInterceptorDiscBounceEffect_Debris;
	//debrisThetaMin = 0;
	//debrisThetaMax = 60;
	//debrisNum = 1;
	//debrisNumVariance = 1;
	//debrisVelocity = 10.0;
	//debrisVelocityVariance = 5.0;

	// Dynamic light
	lightStartRadius = 1;
	lightEndRadius = 5;
	lightStartColor = "1.0 1.0 1.0";
	lightEndColor = "0.0 0.0 0.0";

	shakeCamera = false;
};

