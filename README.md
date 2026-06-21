# System biblioteki – projekt OOP

## Opis tematu

Aplikacja konsolowa modelująca działanie małej biblioteki. System obsługuje
różne typy materiałów bibliotecznych (książki, czasopisma, audiobooki),
czytelników oraz proces wypożyczania i zwrotu. Każdy typ materiału ma inną
liczbę dni wypożyczenia, a biblioteka powiadamia czytelnika o operacjach
(e-mail/SMS) bez wiedzy o konkretnej implementacji powiadomienia.

Projekt zaimplementowany w języku **C#** (`Biblioteka.cs`).

## Lista klas

| Klasa | Odpowiedzialność | Najważniejsze właściwości | Najważniejsze metody |
|---|---|---|---|
| `MaterialBiblioteczny` (abstrakcyjna) | Wspólny kontrakt dla wszystkich materiałów | `Id`, `Tytul`, `Autor`, `CzyDostepny` | `OpisTypu()`, `DniWypozyczenia()`, `Wypozycz()`, `Zwroc()` |
| `Ksiazka` | Materiał typu książka | `LiczbaStron` | `OpisTypu()`, `DniWypozyczenia()` |
| `Czasopismo` | Materiał typu czasopismo | `NumerWydania` | `OpisTypu()`, `DniWypozyczenia()` |
| `Audiobook` | Materiał typu audiobook | `DlugoscMinuty` | `OpisTypu()`, `DniWypozyczenia()` |
| `IPowiadomienie` (interfejs) | Kontrakt na wysyłanie powiadomień | – | `Wyslij(wiadomosc)` |
| `PowiadomienieEmail` | Wysyłka powiadomień e-mail | – | `Wyslij(wiadomosc)` |
| `PowiadomienieSms` | Wysyłka powiadomień SMS | – | `Wyslij(wiadomosc)` |
| `Czytelnik` | Dane czytelnika i jego historia wypożyczeń | `Id`, `Imie`, `Nazwisko`, `email` (prywatne), `Historia` | `UstawEmail()`, `PobierzEmail()` |
| `Wypozyczenie` | Powiązanie czytelnika z materiałem + dane samego wypożyczenia | `Czytelnik`, `Material`, `DniNaZwrot`, `CzyOddane` | `Oddaj()` |
| `Biblioteka` | Zarządzanie materiałami, czytelnikami i wypożyczeniami | listy materiałów / czytelników / wypożyczeń | `DodajMaterial()`, `ZarejestrujCzytelnika()`, `WypozyczMaterial()`, `ZwrocMaterial()`, `PokazMaterialy()` |

## Relacje między klasami

- **Dziedziczenie** – `Ksiazka`, `Czasopismo`, `Audiobook` dziedziczą po
  `MaterialBiblioteczny` (relacja „jest rodzajem").
- **Interfejs** – `PowiadomienieEmail` i `PowiadomienieSms` implementują
  `IPowiadomienie`.
- **Kolekcja (1 → wiele)** – `Czytelnik` ma listę `Historia` (wiele obiektów
  `Wypozyczenie`).
- **Agregacja** – `Biblioteka` grupuje `MaterialBiblioteczny` i `Czytelnik`,
  ale obiekty te mogłyby istnieć niezależnie od biblioteki.
- **Klasa pośrednia (wiele-do-wielu)** – `Wypozyczenie` łączy `Czytelnik`
  z `MaterialBiblioteczny` i przechowuje własne dane powiązania
  (`DniNaZwrot`, `CzyOddane`).
- **Parametr konstruktora / Dependency Injection** – `Biblioteka` dostaje
  obiekt `IPowiadomienie` w konstruktorze, dzięki czemu sposób powiadamiania
  można podmienić bez zmian w klasie `Biblioteka`.
- **Parametr metody** – `WypozyczMaterial()` i `ZwrocMaterial()` przyjmują
  `Czytelnik` i `MaterialBiblioteczny` jako argumenty.
- **Identyfikator** – każdy główny obiekt (`MaterialBiblioteczny`,
  `Czytelnik`) ma unikalne `Id`.

## Cztery zasady OOP

**Enkapsulacja**
Właściwości materiałów i czytelnika są ustawiane tylko przez konstruktor lub
dedykowane metody (`private set` w C#). Pole
`email` w `Czytelnik` jest dodatkowo chronione walidacją w `UstawEmail()` –
nieprawidłowy adres (bez `@`) jest odrzucany.

**Dziedziczenie**
Klasa abstrakcyjna `MaterialBiblioteczny` jest bazą dla trzech klas
szczegółowych: `Ksiazka`, `Czasopismo`, `Audiobook`. Każda z nich rozszerza
bazę o własną właściwość (`LiczbaStron`, `NumerWydania`, `DlugoscMinuty`).

**Polimorfizm**
Metoda `PokazInfo()` w pętli po liście materiałów wywołuje `OpisTypu()` oraz
`DniWypozyczenia()` – ta sama instrukcja wykonuje się inaczej dla książki,
czasopisma i audiobooka, bez sprawdzania typu obiektu.

**Abstrakcja**
Zrealizowana dwukrotnie:
- klasa abstrakcyjna `MaterialBiblioteczny` definiuje kontrakt
  (`OpisTypu()`, `DniWypozyczenia()`), który muszą zrealizować klasy
  pochodne;
- interfejs `IPowiadomienie` definiuje kontrakt `Wyslij()`, niezależny od
  konkretnego kanału powiadomień (e-mail, SMS).

## Przykładowe uruchomienie

Program tworzy bibliotekę, dodaje trzy materiały i jednego czytelnika,
wypożycza dwa materiały, próbuje wypożyczyć już wypożyczoną książkę
(operacja zostaje zablokowana), a następnie zwraca jeden materiał –
wszystko z komunikatami w konsoli pokazującymi zmiany stanu.

```
[1] Pan Tadeusz - Adam Mickiewicz (Ksiazka) - dostepny
[2] Swiat Nauki - Redakcja (Czasopismo nr 5) - dostepny
[3] Wiedzmin - Andrzej Sapkowski (Audiobook (720 min)) - dostepny

E-MAIL: Anna, wypozyczyles "Pan Tadeusz" na 30 dni.
E-MAIL: Anna, wypozyczyles "Wiedzmin" na 14 dni.
Material "Pan Tadeusz" jest juz wypozyczony.
```

## Pliki w repozytorium

- `Program.cs` – kod aplikacji

## Wykorzystanie AI

Asystent AI (Claude) został użyty do zaproponowania pomysłu, wraz z struktórą projektu na
podstawie materiałów z kursu. Udzielił również drobnej pomocy w tworzeniu tego README.
