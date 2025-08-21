<br />
<div align="center">
  <h3 align="center">Echoes</h3>

  <p align="center">
    A plugin for Unity to manage a dynamic NPC social network!
    <br />
    <br />
    <a href="">View Demo</a>
    &middot;
    <a href="https://heig-pdg-groupe-24.github.io/landing-page/">Landing page</a>
</div>


<!-- ABOUT THE PROJECT -->
## About The Project

Develop a Unity plugin to manage a dynamic NPC social network, including:

* Relationships between NPCs (contacts, trust, factions).
* Dynamic opinions (friendship, distrust, hostility, admiration, etc.).
* Rumor system allowing NPCs to propagate information to contacts.
* Dialogue or behavior conditions based on these opinions.

The goal is to provide developers with a reusable, modular, and easy-to-integrate tool to enrich their narrative games, RPGs, or social simulations.

### Functional Requirements

* Create/Edit NPCs via a custom Unity UI.
* Manage contacts (relationship type, trust level, distance).
* Dynamically update an NPC's opinion of the player or another NPC.
* Simulate rumor propagation with delays and reliability based on trust links.
* Expose a simple API (e.g., `npc.GetOpinionOf(player)`, `npc.SpreadRumor(...)`).
* Support factions (alignments that influence initial opinion).
* Support favorites subjects (e.g., favorite food, hobbies) to influence opinions.

### Non-Functional Requirements

* *Performance*: The plugin should support hundreds of NPCs without significantly impacting the framerate.
* *Extensibility*: It should be easy to extend (e.g., adding a new type of emotion/opinion).
* *Compatibility*: Compatible with Unity 6.2 and later.
* *Documentation*: Clear documentation with usage examples.
* *Testability*: The architecture should be decoupled (using patterns like Observer, Strategy, ScriptableObjects).

### Technical choices

We decided to use Unity as the development platform for this plugin, given its popularity and flexibility for game development. Especially because as us developers, we aren't familiar with it and it is a good opportunity to learn.

We will use the following technologies and patterns to implement the plugin:

* *Unity version*: 6.2 or later.
* *Programming Language*: C# (standard Unity).
* *Architecture*:
    * ScriptableObjects to define NPCs, factions, and rumor tables.
    * Graph/Network model to represent contacts.
    * Event System (Observer pattern) to notify opinion changes.
    * Custom Unity Editor to represent NPCs, relationships, and opinions (With Odin Inspector or similar).
    * Data Persistence via JSON or serialized ScriptableObjects.
    * Unit tests with NUnit.

### Built With

This section should list any major frameworks/libraries used to bootstrap your project. Leave any add-ons/plugins for the acknowledgements section. Here are a few examples.

* [![Unity][Unity]][Unity-url]

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

### Installation

<!-- USAGE EXAMPLES -->
## Usage

For a more specific example, you can refer to the video demo available on the [landing page](https://github.com/HEIG-PDG-GROUPE-24/landing-page).

<!-- CONTACT -->
## Contact

Mathieu Rabot / Nomeos - [Github](https://github.com/Nomeos)

Raphaël Perret / rp2709 - [Github](https://github.com/rp2709)

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[Unity]: https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white
[Unity-url]: https://unity.com/
