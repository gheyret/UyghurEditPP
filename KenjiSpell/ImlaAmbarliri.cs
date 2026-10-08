/*
 * Loads the spelling dictionary for each script (UEY/ULY/USY) once, in the background,
 * and keeps it, so switching between tabs in different scripts does not reload it.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace UyghurEditPP
{
	public class ImlaAmbarliri
	{
		readonly Func<Stream> gAch;
		readonly Func<UyghurSpell> gYasa;
		readonly Dictionary<Uyghur.YEZIQ, Task<UyghurSpell>> gYuklesh = new Dictionary<Uyghur.YEZIQ, Task<UyghurSpell>>();

		/// <param name="ach">Opens the dictionary data (word and count per line, in UEY).</param>
		/// <param name="yasa">Creates an empty spell checker.</param>
		public ImlaAmbarliri(Func<Stream> ach, Func<UyghurSpell> yasa)
		{
			gAch = ach;
			gYasa = yasa;
		}

		/// <summary>
		/// Returns the loading (or loaded) dictionary for the script. The first call for a
		/// script starts loading it on a thread-pool thread; a failed load is retried.
		/// </summary>
		public Task<UyghurSpell> Get(Uyghur.YEZIQ yeziq)
		{
			lock(gYuklesh){
				Task<UyghurSpell> task;
				if(!gYuklesh.TryGetValue(yeziq, out task) || task.IsFaulted || task.IsCanceled){
					task = Task.Run(() => {
						UyghurSpell spell = gYasa();
						using(Stream stream = gAch()){
							spell.Load(stream, yeziq);
						}
						return spell;
					});
					gYuklesh[yeziq] = task;
				}
				return task;
			}
		}

		/// <summary>
		/// The dictionaries that have finished loading.
		/// </summary>
		public List<UyghurSpell> Loaded
		{
			get{
				List<UyghurSpell> ret = new List<UyghurSpell>();
				lock(gYuklesh){
					foreach(Task<UyghurSpell> task in gYuklesh.Values){
						if(task.Status == TaskStatus.RanToCompletion){
							ret.Add(task.Result);
						}
					}
				}
				return ret;
			}
		}
	}
}
