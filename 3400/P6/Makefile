TEX := latexmk

all: submission

submission: submission.tex
	$(TEX) submission.tex -pdf submission.pdf

clean:
	$(TEX) -c submission

prune:
	$(TEX) -CA submission
