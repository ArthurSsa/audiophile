using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

/* * PROJETO FINAL: audiophile - Showroom Master (Versão Celular Estável)
 * - Início Imediato: O jogo começa sem menus para evitar erros de clique.
 * - HUD Checklist: Topo central com textos curtos e feedback visual (Verde).
 * - Animação de Mão: Alcance longo (0.75), Z: -42.53, X/Y invertidos (25, 0).
 * - Alvo Final: Telemóvel/Celular (Mecânica original restaurada).
 */
public class GerenciadorAudiophile : MonoBehaviour
{
    [Header("HUD e Checklist")]
    public TextMeshProUGUI textoTarefa; 
    public Image iconeCheck;            // UI Image (quadrado branco)
    public GameObject setaIndicadora;    

    [Header("Configurações da Luva (View Model)")]
    public Transform luvaJogador; 
    public float intensidadeSway = 0.012f; 
    public float limiteSway = 0.035f;      
    public float suavidadeSway = 12f; 
    public float forcaClique = 0.75f;    
    public Vector3 rotacaoClique = new Vector3(25f, 0f, -42.53f); 

    [Header("Referências de Cena")]
    public AudioSource somVinil;
    public Transform alvoVinil;
    public VideoPlayer videoTV; 
    public AudioSource[] colunasTV; 
    public GameObject telaDaTV; 
    public Transform alvoTV;
    public AudioSource somFone;
    public Transform alvoCelular; // Restaurado: Alvo é o Celular
    public GameObject modeloFone3D; // O fone que some da mesa ao equipar

    private int faseAtual = 0; 
    private bool missaoConcluida = false;
    private bool foneEquipado = false;
    private Vector3 posAncora;
    private Quaternion rotAncora;
    private bool estaAnimando = false;

    void Start()
    {
        // Forçar visão ao horizonte no início
        Camera.main.transform.localRotation = Quaternion.identity;
        
        // Esconder cursor para jogabilidade imediata
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Reset de estados sonoros e visuais
        if (somVinil) somVinil.Play();
        if (telaDaTV) telaDaTV.SetActive(false);
        foreach(var s in colunasTV) if(s) s.Stop();
        if (somFone) somFone.Stop();

        // Capturar âncora da luva (View Model)
        if (luvaJogador)
        {
            posAncora = luvaJogador.localPosition;
            rotAncora = luvaJogador.localRotation;
        }

        if (iconeCheck) iconeCheck.color = Color.white;
        AtualizarHUD();
    }

    void Update()
    {
        if (!estaAnimando) AplicarSwayLuva();

        // Animação da Seta Indicadora
        if (setaIndicadora && setaIndicadora.activeSelf)
        {
            setaIndicadora.transform.Rotate(Vector3.up, 180 * Time.deltaTime);
            setaIndicadora.transform.position += new Vector3(0, Mathf.Sin(Time.time * 5) * 0.0005f, 0);
        }

        // Interação via Clique (Raycast Centralizado)
        if (Input.GetMouseButtonDown(0))
        {
            if (luvaJogador && !estaAnimando) StartCoroutine(ExecutarAnimacaoClique());

            Ray raio = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
            if (Physics.Raycast(raio, out RaycastHit hit))
            {
                ValidarInteracao(hit.transform);
            }
        }
    }

    void ValidarInteracao(Transform clicado)
    {
        if (missaoConcluida) { LogicaLivre(clicado); return; }

        bool objetivoConcluido = false;

        switch (faseAtual)
        {
            case 0: // Desligar Vinyl
                if (clicado == alvoVinil || clicado.IsChildOf(alvoVinil)) {
                    somVinil.Stop();
                    objetivoConcluido = true;
                }
                break;
            case 1: // Ligar TV
                if (clicado == alvoTV || clicado.IsChildOf(alvoTV)) {
                    MudarEstadoTV(true);
                    objetivoConcluido = true;
                }
                break;
            case 2: // Desligar TV
                if (clicado == alvoTV || clicado.IsChildOf(alvoTV)) {
                    MudarEstadoTV(false);
                    objetivoConcluido = true;
                }
                break;
            case 3: // Equipar Fones via Celular
                if (clicado == alvoCelular || clicado.IsChildOf(alvoCelular)) {
                    MecanicaEquipar(true);
                    objetivoConcluido = true;
                }
                break;
        }

        if (objetivoConcluido) StartCoroutine(ProcessarCheckHUD());
    }

    IEnumerator ProcessarCheckHUD()
    {
        if (iconeCheck) iconeCheck.color = Color.green;
        yield return new WaitForSeconds(0.8f);
        
        faseAtual++;
        if (faseAtual > 3) missaoConcluida = true;
        
        if (iconeCheck) iconeCheck.color = Color.white;
        AtualizarHUD();
    }

    void AtualizarHUD()
    {
        if (missaoConcluida)
        {
            textoTarefa.text = "Showroom Finalizado!";
            if (setaIndicadora) setaIndicadora.SetActive(false);
            if (iconeCheck) iconeCheck.gameObject.SetActive(false);
            return;
        }

        switch (faseAtual)
        {
            case 0: textoTarefa.text = "Desligar o Vinyl"; MoverSeta(alvoVinil); break;
            case 1: textoTarefa.text = "Ligar a TV"; MoverSeta(alvoTV); break;
            case 2: textoTarefa.text = "Desligar a TV"; MoverSeta(alvoTV); break;
            case 3: textoTarefa.text = "Equipar Fones"; MoverSeta(alvoCelular); break;
        }
    }

    void MoverSeta(Transform alvo)
    {
        if (setaIndicadora && alvo)
        {
            setaIndicadora.SetActive(true);
            setaIndicadora.transform.position = alvo.position + Vector3.up * 0.6f;
        }
    }

    void MudarEstadoTV(bool ligar)
    {
        if (telaDaTV) telaDaTV.SetActive(ligar);
        if (ligar) { videoTV.Play(); foreach(var s in colunasTV) s.Play(); }
        else { videoTV.Stop(); foreach(var s in colunasTV) s.Stop(); }
    }

    void MecanicaEquipar(bool equipar)
    {
        foneEquipado = equipar;
        if (modeloFone3D) modeloFone3D.SetActive(!equipar);
        if (equipar) somFone.Play(); else somFone.Stop();
    }

    void LogicaLivre(Transform clicado)
    {
        if (clicado == alvoVinil || clicado.IsChildOf(alvoVinil)) {
            if (somVinil.isPlaying) somVinil.Stop(); else somVinil.Play();
        }
        else if (clicado == alvoTV || clicado.IsChildOf(alvoTV)) MudarEstadoTV(!videoTV.isPlaying);
        else if (clicado == alvoCelular || clicado.IsChildOf(alvoCelular)) MecanicaEquipar(!foneEquipado);
    }

    void AplicarSwayLuva()
    {
        if (!luvaJogador) return;
        float mX = -Input.GetAxis("Mouse X") * intensidadeSway;
        float mY = -Input.GetAxis("Mouse Y") * intensidadeSway;
        mX = Mathf.Clamp(mX, -limiteSway, limiteSway);
        mY = Mathf.Clamp(mY, -limiteSway, limiteSway);
        luvaJogador.localPosition = Vector3.Lerp(luvaJogador.localPosition, posAncora + new Vector3(mX, mY, 0), Time.deltaTime * suavidadeSway);
    }

    IEnumerator ExecutarAnimacaoClique()
    {
        estaAnimando = true;
        // Calcula a direção de alcance baseada no forward da câmera
        Vector3 dir = luvaJogador.parent.InverseTransformDirection(Camera.main.transform.forward);
        Vector3 targetPos = posAncora + (dir * forcaClique);
        Quaternion targetRot = rotAncora * Quaternion.Euler(rotacaoClique);

        float t = 0;
        // Movimento de Ida
        while (t < 1) {
            t += Time.deltaTime / 0.12f;
            luvaJogador.localPosition = Vector3.Lerp(posAncora, targetPos, t);
            luvaJogador.localRotation = Quaternion.Slerp(rotAncora, targetRot, t);
            yield return null;
        }
        // Movimento de Volta
        t = 0;
        while (t < 1) {
            t += Time.deltaTime / 0.35f;
            luvaJogador.localPosition = Vector3.Lerp(targetPos, posAncora, t);
            luvaJogador.localRotation = Quaternion.Slerp(targetRot, rotAncora, t);
            yield return null;
        }
        estaAnimando = false;
    }
}