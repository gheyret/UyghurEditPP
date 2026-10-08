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
		
		// Words and corrections the user added in this session, and the dictionaries that are
		// ready. Both are guarded by gQulup, so a dictionary that finishes loading while the
		// user adds a word either gets it from gSozler or from SozQoshuldi, never neither.
		readonly object gQulup = new object();
		readonly List<string> gSozler = new List<string>();
		readonly List<string[]> gTuzitishler = new List<string[]>();
		readonly List<UyghurSpell> gTeyyar = new List<UyghurSpell>();

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
						lock(gQulup){
							foreach(string soz in gSozler){
								spell.IshletkuchiSozQosh(soz);
							}
							foreach(string[] tuz in gTuzitishler){
								spell.XataToghraQosh(tuz[0], tuz[1]);
							}
							gTeyyar.Add(spell);
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
				lock(gQulup){
					return new List<UyghurSpell>(gTeyyar);
				}
			}
		}
		
		/// <summary>
		/// The user marked soz as correct in menbe's script (menbe has already saved it).
		/// Every other dictionary learns it, including those that are still loading.
		/// </summary>
		public void SozQoshuldi(string soz, UyghurSpell menbe)
		{
			lock(gQulup){
				gSozler.Add(soz);
				foreach(UyghurSpell spell in gTeyyar){
					if(spell != menbe){
						spell.IshletkuchiSozQosh(soz);
					}
				}
			}
		}
		
		/// <summary>
		/// The user chose toghra as the correction of xata in menbe's script.
		/// </summary>
		public void TuzitishQoshuldi(string xata, string toghra, UyghurSpell menbe)
		{
			lock(gQulup){
				gTuzitishler.Add(new[]{ xata, toghra });
				foreach(UyghurSpell spell in gTeyyar){
					if(spell != menbe){
						spell.XataToghraQosh(xata, toghra);
					}
				}
			}
		}
	}
}
