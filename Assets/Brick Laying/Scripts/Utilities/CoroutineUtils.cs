using UnityEngine;
using System;
using System.Collections;
using Lean.Transition;

public static class CoroutineUtils
{

	/*
	 * Usage: StartCoroutine(CoroutineUtils.Chain(...))
	 * For example:
	 *     StartCoroutine(CoroutineUtils.Chain(
	 *         CoroutineUtils.Do(() => Debug.Log("A")),
	 *         CoroutineUtils.WaitForSeconds(2),
	 *         CoroutineUtils.Do(() => Debug.Log("B"))));
	 */
	public static IEnumerator Chain(params IEnumerator[] actions)
	{
		foreach (IEnumerator action in actions) {
			yield return GameManager.Instance.StartCoroutine(action);
		}
	}

	/*
	 * Usage: StartCoroutine(CoroutineUtils.DelaySeconds(action, delay))
	 * For example:
	 *     StartCoroutine(CoroutineUtils.DelaySeconds(
	 *         () => DebugUtils.Log("2 seconds past"),
	 *         2);
	 */
	public static IEnumerator DelaySeconds(Action action, float delay)
	{
		yield return new WaitForSeconds(delay);
		action();
	}

	public static IEnumerator WaitForSeconds(float time)
	{
		yield return new WaitForSeconds(time);
	}

	public static IEnumerator WaitForSecondsRealtime(float time)
	{
		yield return new WaitForSecondsRealtime(time);
	}

	public static IEnumerator Do(Action action)
	{
		action();
		yield return 0;
	}

	public static IEnumerator DoNextFrame(Action action)
	{
		yield return new WaitForEndOfFrame();
		action();
	}

	/*
	 * Usage: StartCoroutine(CoroutineUtils.LinearAction(time, action))
	 * For example:
	 *     StartCoroutine(CoroutineUtils.LinearAction(3, (weight) => {
	 * 			position = lerp (start, end, weight); 
	 *        });
	 */
	public static IEnumerator LinearAction(float time, Action<float> callback)
	{
		float elapsed = 0;
		while (elapsed < time) {

			callback(elapsed / time);

			yield return new WaitForEndOfFrame();
			elapsed += Time.deltaTime;
		}

		callback(1);
	}

	public static IEnumerator AudioFade(AudioSource audioSource, float value, float duration, LeanEase ease) 
	{
		float startVolume = audioSource.volume;
		yield return LinearAction(duration, (w) => {
			audioSource.volume = Mathf.Lerp(startVolume, value, LeanMethod.Smooth(ease, w));
		});
	}

	/*
	 * Usage: StartCoroutine(CoroutineUtils.IntervalAction(interval, duration, action))
	 * For example:
	 *     StartCoroutine(CoroutineUtils.IntervalAction(0.2f, 3f, (n) => {
	 * 			Debug.Log("Hooray " + n + " times");
	 *        });
	 */
	public static IEnumerator IntervalAction(float interval, float duration, Action<int> callback)
	{
		float elapsed = 0;
		int count = -1;
		while (elapsed < duration) {

			//callback(elapsed / duration);

			if (Mathf.FloorToInt(elapsed / interval) > count)
				callback(++count);

			yield return new WaitForEndOfFrame();
			elapsed += Time.deltaTime;
		}

		//callback(1);
	}

	public static IEnumerator SmoothMove(Transform target, Vector2 start, Vector2 end, float time)
	{
		// Ease out
		float t = 0;
		while (t <= 1.0f) {
			t += Time.deltaTime / time;
			target.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0.0f, 1.0f, t));
			yield return new WaitForEndOfFrame();
		}
	}
}
