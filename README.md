# Pako W Salonie - Projekt Edukacyjnej Gry Interaktywnej dla Dzieci (Unity / C#)

Projekt własny (WIP) realizowany samodzielnie jako Solo Developer. 
Gra skupia się na minigrach logiczno-edukacyjnych dla najmłodszych, łącząc kod, autorską grafikę 2D oraz dźwięk.

## Kluczowe cechy techniczne:
* Architektura oparta na wzorcu Singleton (zarządzanie pauzą i stanami)
* Logika Drag & Drop UI (IDragHandler, IEndDragHandler)
* Dynamiczny, inteligentny system audio chroniący przed nakładaniem się linii lektorskich
* System zapisu postępów oparty o PlayerPrefs

## Technologie:
* Unity & C#
* GIMP (ręcznie rysowane assety graficzne)
* Generative AI Workflow (generowanie i czyszczenie audio)

## Zakres i osiągnięcia:
* **Architektura Kodu i Logika (C#):** Projektowanie modułowej i skalowalnej architektury gry w paradygmacie obiektowym z wykorzystaniem wzorca projektowego Singleton (zarządzanie stanami gry i menu pauzy).
* **System Dedykowanych Minigier:** Implementacja mechanik minigier logiczno-edukacyjnych, w tym zaawansowanego systemu przeciągania elementów (Drag & Drop UI) z weryfikacją warunków sukcesu/błędu na podstawie algorytmu dopasowania par tagów i pozycji (np. dopasowywanie książek do odpowiednich półek).
* **Interdyscyplinarny Pipeline (Solo Dev):** Samodzielne przygotowanie 100% assetów do gry – od koncepcji mechaniki, przez ręczne tworzenie i eksport grafiki 2D (GIMP), aż po inżynierię promptów i czyszczenie ścieżek audio wygenerowanych za pomocą narzędzi AI.
* **Dynamiczny System Audio i Narracji:** Zaprogramowanie inteligentnego managera audio sterującego odtwarzaniem linii lektorskich (narratora), zawierającego funkcje automatycznego wyciszania innych źródeł dźwięku (AudioSource) w celu uniknięcia nakładania się ścieżek głosowych oraz mechanizm wyciszania globalnego (AudioListener).
* **Zapis Stanu Gry (Persistence):** Implementacja prostego systemu zapisu postępów i odblokowywania mebli-zadań za pomocą PlayerPrefs.
