//UNITY_SHADER_NO_UPGRADE
#ifndef CustomWater
#define CustomWater

#define delta 1.1


void WaveFunction_float(float2 uvs, float2 resolution, out float4 fragColor){


    int2 fragCoord = uvs * resolution;

    float2 editUVs = fragCoord/resolution;
    float pressure =  SAMPLE_TEXTURE2D(_MainTexture, sampler_MainTexture, editUVs).x;
    float pVel =  SAMPLE_TEXTURE2D(_MainTexture, sampler_MainTexture, editUVs).y;

    int2 editCoord = fragCoord + int2(1,0);
    editUVs = fragCoord/resolution;
    float p_right = SAMPLE_TEXTURE2D(_MainTexture, sampler_MainTexture, editUVs).x;

    editCoord = fragCoord + int2(-1,0);
    editUVs = fragCoord/resolution;
    float p_left = SAMPLE_TEXTURE2D(_MainTexture, sampler_MainTexture, editUVs).x;

    editCoord = fragCoord + int2(0,1);
    editUVs = fragCoord/resolution;
    float p_up = SAMPLE_TEXTURE2D(_MainTexture, sampler_MainTexture, editUVs).x;

    editCoord = fragCoord + int2(0,-1);
    editUVs = fragCoord/resolution;
    float p_down = SAMPLE_TEXTURE2D(_MainTexture, sampler_MainTexture, editUVs).x;

    // Apply horizontal wave function
    pVel += delta * (-2.0 * pressure + p_right + p_left) / 4.0;
    // Apply vertical wave function (these could just as easily have been one line)
    pVel += delta * (-2.0 * pressure + p_up + p_down) / 4.0;

    // Change pressure by pressure velocity
    pressure += delta * pVel;

    // "Spring" motion. This makes the waves look more like water waves and less like sound waves.
    pVel -= 0.005 * delta * pressure;
    
    // Velocity damping so things eventually calm down
    pVel *= 1.0 - 0.002 * delta;
    
    // Pressure damping to prevent it from building up forever.
    pressure *= 0.999;
    
    //x = pressure. y = pressure velocity. Z and W = X and Y gradient
    fragColor.xyzw = float4(pressure, pVel, (p_right - p_left) / 2.0, (p_up - p_down) / 2.0);

}

void EmitWave_float(float2 pressed_uvs, float2 pixel_uvs, float2 resolution, float4 col, out float4 output_col){
    if(pressed_uvs.x == 0.0 && pressed_uvs.y == 0.0){
        return;

    }

    output_col = col;

    int2 fragCoord = pixel_uvs * resolution;
    int2 pressed_Coords = pressed_uvs * resolution;

    float dist = distance(fragCoord, pressed_Coords);
    if (dist <= 20.0) {
        output_col.x = col.x + (1.0 - dist / 20.0);
    }

}

#endif