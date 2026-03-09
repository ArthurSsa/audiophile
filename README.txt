Audiophile - Showroom Hi-Fi em Realidade Virtual

Desenvolvedor: Arthur

Status do Projeto: Finalizado (Gold Master)

Plataforma: Unity 6 / Meta XR SDK (Android)

📋 Descrição do Projeto

O Audiophile é um simulador imersivo que explora o conceito de "Design Sensorial" e "Silêncio Acústico". O utilizador é colocado num quarto técnico e deve gerir fontes de ruído (Vinyl e TV) para atingir a pureza sonora necessária para utilizar os fones de alta fidelidade.

🛠️ Desafios Técnicos Superados

1. View Model (Luva 3D)

Clipping: Resolvido o corte da malha através do ajuste do Near Clipping Plane da câmara para 0.01.

Sway & Clamp: Implementado sistema de balanço inercial que mantém a luva no campo de visão, independentemente da velocidade do jogador (ajustada para 6.0).

Animação Procedural: O clique combina translação frontal (0.75f) e rotação em Z (-42.53f) para simular o movimento tátil de alcance.

2. Lógica de Sequência e Silêncio

Implementado um sistema de fases (State Machine) que obriga o utilizador a silenciar a sala antes de permitir a ativação do áudio 2D imersivo nos fones.

3. UI e Navegação (UX)

HUD Checklist: Localizado no topo central, com textos curtos e feedback visual imediato (ícone muda para verde via Corrotina C#).

Navegação Assistida: Seta flutuante (Seta_Indicadora) que se desloca dinamicamente para o próximo alvo da missão.

📂 Organização de Assets

/Videos: Clips de vinil para a Render Texture da TV.

/Musics: Áudios espaciais e 2D.

/Scripts: Lógica principal no GerenciadorAudiophile.cs.

/Materials: Materiais extraídos e calibrados para o Universal Render Pipeline (URP).

Projeto desenvolvido para a Residência em TIC 29 - Trilha Web 3.0.