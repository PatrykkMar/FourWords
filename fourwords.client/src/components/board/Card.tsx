import { useContext } from "react";
import type { CardDTO } from "../../models/dtos";
import { BoardContext } from "../../context/BoardContext";
import styles from "./card.module.css";
import { useTranslation } from "react-i18next";

interface CardProps {
    card: CardDTO;
}

export default function Card({ card }: CardProps) {
    const { t } = useTranslation();
    const boardContext = useContext(BoardContext);

    const originalWords = [card.wordTop, card.wordRight, card.wordBottom, card.wordLeft];
    const rotation = (card.currentRotation ?? 0) % 4;

    const rotatedWords = [
        ...originalWords.slice(4 - rotation),
        ...originalWords.slice(0, 4 - rotation)
    ];

    const [currentTop, currentRight, currentBottom, currentLeft] = rotatedWords;

    const handleDragStart = (e: React.DragEvent) => {
        e.dataTransfer.setData("text/plain", card.gameRoomCardId.toString());
        e.dataTransfer.effectAllowed = "move";
    };

    const handleRotateClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (boardContext?.rotateCard && card.gameRoomCardId) {
            boardContext.rotateCard({ gameRoomCardId: card.gameRoomCardId });
        }
    };

    return (
        <div
            className={styles.slot}
            draggable={boardContext?.board?.cardsActive && !card.isCorrect}
            onDragStart={handleDragStart}
        >
            {card ? (
                <div className={`${styles.card} ${card.isCorrect ? styles.correctCard : ""}`}>
                    <div className={styles.wordTop}>{currentTop}</div>
                    <div className={styles.wordRight}>{currentRight}</div>
                    <div className={styles.wordBottom}>{currentBottom}</div>
                    <div className={styles.wordLeft}>{currentLeft}</div>

                    {!card.isCorrect && (
                        (
                            boardContext?.board?.cardsActive && (
                                <button
                                    className={styles.rotateBtn}
                                    onClick={handleRotateClick}
                                    title={t("card.rotateTooltip")}
                                >
                                    ↻
                                </button>
                            )
                        )
                    )}

                </div>
            ) : (
                <div className={styles.emptySlot}>{t("card.emptySlot")}</div>
            )}
        </div>
    );
}