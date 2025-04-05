using System;
using UnityEngine;

public static class AnimatorExtensions
{
    public static bool HasAnimation<TEnum>(this Animator animator, TEnum enumValue) where TEnum : Enum
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            return false;
        }

        string animationName = enumValue.ToString();
        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        int clipsLength = clips.Length;
        for (int i = 0; i < clipsLength; i++)
        {
            if (clips[i].name.Contains(animationName))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasParam(this Animator animator, string paramName)
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        AnimatorControllerParameter[] parameters = animator.parameters;

        int parametersLength = animator.parameters.Length;
        for (int i = 0; i < parametersLength; i++)
        {
            if (parameters[i].name == paramName)
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasParam<TEnum>(this Animator animator, TEnum parameter) where TEnum : Enum
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        AnimatorControllerParameter[] parameters = animator.parameters;

        string parameterName = parameter.ToString();

        int parametersLength = animator.parameters.Length;
        for (int i = 0; i < parametersLength; i++)
        {
            if (parameters[i].name == parameterName)
            {
                return true;
            }
        }

        return false;
    }

    public static bool SetBool<TEnum>(this Animator animator, TEnum parameter, bool state) where TEnum : Enum
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        string parameterName = parameter.ToString();

        if (animator.HasParam(parameterName))
        {
            animator.SetBool(parameterName, state);
            return true;
        }

        return false;
    }

    public static bool SetFloat<TEnum>(this Animator animator, TEnum parameter, float value) where TEnum : Enum
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        string parameterName = parameter.ToString();

        if (animator.HasParam(parameterName))
        {
            animator.SetFloat(parameterName, value);
            return true;
        }

        return false;
    }

    public static bool GetBool<TEnum>(this Animator animator, TEnum parameter) where TEnum : Enum
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        string parameterName = parameter.ToString();

        return animator.GetBool(parameterName);
    }

    public static void SetTrigger<TEnum>(this Animator animator, TEnum parameter) where TEnum : Enum
    {
        if (animator.runtimeAnimatorController == null)
        {
            return;
        }

        string parameterName = parameter.ToString();

        animator.SetTrigger(parameterName);
    }

    public static void ResetTrigger<TEnum>(this Animator animator, TEnum parameter) where TEnum : Enum
    {
        if (animator.runtimeAnimatorController == null)
        {
            return;
        }

        string parameterName = parameter.ToString();

        animator.ResetTrigger(parameterName);
    }

    public static AnimationClip GetAnimation<TEnum>(this Animator animator, TEnum animationKey) where TEnum : Enum
    {
        string animationKeyAsString = animationKey.ToString();

        if (animator == null || animator.runtimeAnimatorController == null)
        {
            return null;
        }

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        int clipsLength = clips.Length;
        for (int i = 0; i < clipsLength; i++)
        {
            AnimationClip clip = clips[i];
            if (clip.name.Contains(animationKeyAsString))
            {
                return clip;
            }
        }

        return null;
    }

    public static bool TryGetAnimation<TEnum>(this Animator animator, TEnum value, out AnimationClip clip) where TEnum : Enum
    {
        clip = GetAnimation(animator, value);
        return clip != null;
    }
}
