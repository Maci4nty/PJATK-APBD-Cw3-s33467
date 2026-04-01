using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        var studenciZWarszawy = DaneUczelni.Studenci
            .Where(s => s.Miasto == "Warsaw");

        foreach (var student in studenciZWarszawy)
        {
            yield return $"{student.NumerIndeksu}, {student.Imie}, {student.Nazwisko}, {student.Miasto}";
        }
    }
    
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        var emails = DaneUczelni.Studenci
            .Select(s => s.Email);

        foreach (var email in emails)
        {
            yield return email;
        }
    }

    
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        var sortedSurnamesAndNames = DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie);


        foreach (var student in sortedSurnamesAndNames)
        {
            yield return $"{student.NumerIndeksu}, {student.Nazwisko}, {student.Imie}";
        }
    }

    
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var first = DaneUczelni.Przedmioty.FirstOrDefault(f => f.Kategoria == "Analytics");
        if (first != null)
        {
            yield return $"{first.Nazwa}, {first.DataStartu}";
        }
        else
        {
            yield return "Taki przedmiot nie istnieje";
        }
    }

   
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        bool istnieje = DaneUczelni.Zapisy.Any(x => x.CzyAktywny == false);
        if (istnieje)
        {
            yield return "True";
        }
        else
        {
            yield return "False";
        }
    }
    
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        bool wszyscy = DaneUczelni.Prowadzacy.All(x => !string.IsNullOrWhiteSpace(x.Katedra));

        if (wszyscy)
        {
            yield return "True";
        }
        else
        {
            yield return "False";
        }
    }

    
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var total = DaneUczelni.Zapisy.Count(z => z.CzyAktywny);
        yield return $"{total}";
    }

    
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        var unique = DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .OrderBy(m => m);

        foreach (var town in unique)
            yield return town;
    }
    
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        var zapisy = DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3);
        
        foreach (var zap in zapisy)
        {
            yield return $"{zap.DataZapisu}, {zap.StudentId}, {zap.PrzedmiotId}";
        }
    }
    
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        var przedmioty = DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2);

        foreach (var przedmiot in przedmioty)
        {
            yield return $"{przedmiot.Nazwa}, {przedmiot.Kategoria}";
        }
    }
    
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var joined = DaneUczelni.Studenci.Join(
            DaneUczelni.Zapisy,
            stud => stud.Id,
            zap => zap.StudentId,
            (stud, zap) => new
            {
                student = stud,
                zapDate = zap.DataZapisu
            }
        );

        foreach (var stud in joined)
        {
            yield return $"{stud.student.Imie}, {stud.student.Nazwisko}, {stud.zapDate}";
        }
    }

    
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        var doubleJoined = DaneUczelni.Zapisy
            .SelectMany(
                zap => DaneUczelni.Studenci.Where(s => s.Id == zap.StudentId),
                (zap, stud) => new
                {
                    zap,
                    stud,
                }
            )
            .SelectMany(tmp => DaneUczelni.Przedmioty.Where(p => p.Id == tmp.zap.PrzedmiotId),
                (tmp, przed) => new
                {
                    tmp.stud,
                    przed
                }
            );

        foreach (var res in doubleJoined)
        {
            yield return $"{res.stud.Imie}, {res.stud.Nazwisko}, {res.przed.Nazwa}";
        }
    }

    
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var joined = DaneUczelni.Zapisy
            .GroupBy(z => z.PrzedmiotId)
            .Join(
            DaneUczelni.Przedmioty,
            zap => zap.Key,
            przed => przed.Id,
            (zap, przed) => new
            {
                przedmiot = przed.Nazwa,
                counted = zap.Count()
            }
        );

        foreach (var res in joined)
        {
            yield return $"{res.przedmiot}, {res.counted}";
        }
    }
    
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var joined = DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .GroupBy(z => z.PrzedmiotId)
            .Join(
                DaneUczelni.Przedmioty,
                zap => zap.Key,
                przed => przed.Id,
                (zap, przed) => new
                {
                    przedmiot = przed.Nazwa,
                    avg = zap.Average(z => z.OcenaKoncowa.Value)
                }
            );

        foreach (var res in joined)
        {
            yield return $"{res.przedmiot}, {res.avg}";
        }
    }

    
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var stats = DaneUczelni.Prowadzacy.Select(p => new
        {
            Surname = $"{p.Imie}, {p.Nazwisko}",
            counted = DaneUczelni.Przedmioty.Count(przed => przed.ProwadzacyId == p.Id)
        });

        foreach (var stat in stats)
        {
            yield return $"{stat.Surname}, {stat.counted}";
        }
    }
    
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var stat = DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .GroupBy(z => z.StudentId)
            .Join(
                DaneUczelni.Studenci,
                zap => zap.Key,
                stud => stud.Id,
                (zap, stud) => new
                {
                    zapis = zap.Max(z => z.OcenaKoncowa.Value),
                    student = $"{stud.Imie}, {stud.Nazwisko}",
                }
            );
        foreach (var res in stat)
        {
            yield return $"{res.student}, {res.zapis}";
        }
    }

    
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var zapisani = DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .GroupBy(z => z.StudentId)
            .Where(g => g.Count() > 1)
            .Join(
                DaneUczelni.Studenci,
                zap => zap.Key,
                stud => stud.Id,
                (zap, stud) => new
                {
                    student = $"{stud.Imie}, {stud.Nazwisko}",
                    counted = zap.Count()
                }
            );

        foreach (var res in zapisani)
        {
            yield return $"{res.student}, {res.counted}";
        }
    }

   
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var res = DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .Join(
                DaneUczelni.Zapisy,
                przed => przed.Id,
                zap => zap.PrzedmiotId,
                (przed, zap) => new
                {
                    przed = przed.Nazwa,
                    zap,
                }
            )
            .GroupBy(joined => joined.przed)
            .Where(g => g.All(item => !item.zap.OcenaKoncowa.HasValue))
            .Select(g => g.Key);

        foreach (var nazwa in res)
        {
            yield return $"{nazwa}";
        }
    }
    
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        var stat = DaneUczelni.Prowadzacy
            .Select(p =>
            {
                var oceny = DaneUczelni.Przedmioty
                    .Where(przed => przed.ProwadzacyId == p.Id)
                    .Join(
                        DaneUczelni.Zapisy,
                        przed => przed.Id,
                        zap => zap.PrzedmiotId,
                        (przed, zap) => zap
                    )
                    .Where(z => z.OcenaKoncowa.HasValue)
                    .Select(z => z.OcenaKoncowa.Value);
                return new
                {
                    fullName = $"{p.Imie}, {p.Nazwisko}",
                    avg = oceny.Any() ? (double?)oceny.Average() : null,
                };
            });

        foreach (var res in stat) 
        {
            string resOceny = res.avg.HasValue ? $"{res.avg}" : "";
            yield return $"{res.fullName}, {resOceny}";
        }
    }

    
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        var stat = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                s => s.Id,
                z => z.StudentId,
                (s, z) => s.Miasto
            )
            .GroupBy(miasto => miasto)
            .Select(grupa => new
            {
                town = grupa.Key,
                counted = grupa.Count()
            })
            .OrderByDescending(x => x.counted);

        foreach (var res in stat)
        {
            yield return $"{res.town}, {res.counted}";
        }
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
