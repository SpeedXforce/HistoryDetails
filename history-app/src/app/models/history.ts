export interface CreateHistoryDTO {
    historyId: string;
    timestamp: string;
    value: number;
    status: string;
}

export interface SearchHistoryDTO{
    historyId: string;
    fromDate: string;
    toDate: string,
    minute: number;
}

export interface HistoryResponseDTO{
    id: number;
    meterName: string;
    timestamp: string;
    statusTag: string;
    value: number;
}

export interface UpdateHistoryDTO{
id: number;
statusTag: string;
value: number;
}