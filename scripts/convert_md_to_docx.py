#!/usr/bin/env python3
"""Converte arquivos Markdown para DOCX usando pandoc.

Uso:
  python3 scripts/convert_md_to_docx.py --input docs --output docs_out

Requisitos:
  - pandoc (instale com `brew install pandoc` ou visite https://pandoc.org)
"""
from pathlib import Path
import argparse
import shutil
import subprocess
import sys


def find_md_files(src: Path):
    return sorted(p for p in src.rglob("*.md") if p.is_file())


def has_pandoc() -> bool:
    return shutil.which("pandoc") is not None


def convert(md_path: Path, out_path: Path, reference_doc: Path | None = None) -> None:
    out_path.parent.mkdir(parents=True, exist_ok=True)
    cmd = ["pandoc", str(md_path), "-o", str(out_path)]
    if reference_doc:
        cmd += ["--reference-doc", str(reference_doc)]
    subprocess.run(cmd, check=True)


def main():
    parser = argparse.ArgumentParser(description="Converter Markdown → DOCX (usa pandoc)")
    parser.add_argument("--input", "-i", default="docs", help="Pasta de origem com .md")
    parser.add_argument("--output", "-o", default="docs_out", help="Pasta destino para .docx")
    parser.add_argument("--reference", "-r", default=None, help="(Opcional) template .docx de referência")
    args = parser.parse_args()

    src = Path(args.input).resolve()
    dst = Path(args.output).resolve()
    reference = Path(args.reference).resolve() if args.reference else None

    if not src.exists() or not src.is_dir():
        print(f"Erro: pasta de entrada não existe: {src}")
        sys.exit(2)

    if not has_pandoc():
        print("Erro: 'pandoc' não foi encontrado no PATH. Instale-o com 'brew install pandoc' ou veja https://pandoc.org")
        sys.exit(3)

    md_files = find_md_files(src)
    if not md_files:
        print(f"Nenhum arquivo .md encontrado em: {src}")
        return

    succeeded = 0
    failed = 0
    for md in md_files:
        rel = md.relative_to(src)
        out_file = dst.joinpath(rel.parent, md.stem + ".docx")
        try:
            print(f"Convertendo: {md} → {out_file}")
            convert(md, out_file, reference)
            succeeded += 1
        except subprocess.CalledProcessError as e:
            print(f"Falha ao converter {md}: {e}")
            failed += 1

    print(f"Concluído. Sucesso: {succeeded}. Falhas: {failed}.")


if __name__ == "__main__":
    main()
