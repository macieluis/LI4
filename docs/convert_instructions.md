# Instruções: Converter Markdown para DOCX

Este guia mostra como usar o script `scripts/convert_md_to_docx.py` para converter arquivos `.md` em `.docx` usando `pandoc`.

Pré-requisitos
- macOS: instale o Pandoc (ex.: `brew install pandoc`) ou baixe em https://pandoc.org

Uso básico
1. No terminal, vá para a raiz do repositório.
2. Execute:

```bash
python3 scripts/convert_md_to_docx.py --input docs --output docs_out
```

Isso irá procurar recursivamente por `*.md` dentro da pasta `docs` e gerar arquivos `.docx` preservando a estrutura de pastas em `docs_out`.

Opções úteis
- `--input` / `-i`: pasta de origem (padrão: `docs`)
- `--output` / `-o`: pasta destino (padrão: `docs_out`)
- `--reference` / `-r`: caminho para um documento `.docx` de referência (modelo de estilos) para o Pandoc

Exemplo com template de referência:

```bash
python3 scripts/convert_md_to_docx.py -i docs -o docs_out -r templates/reference.docx
```

Notas
- Se `pandoc` não estiver disponível no PATH o script exibirá instruções para instalação.
- Para controle avançado (PDF, HTML, opções de saída), use diretamente o `pandoc`.
