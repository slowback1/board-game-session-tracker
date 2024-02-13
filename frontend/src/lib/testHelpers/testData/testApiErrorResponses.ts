import type { ApiResponse } from '$lib/api/baseApi';

export const testErrorApiResponse: ApiResponse<any> = {
	errors: ['ERROR!'],
	response: undefined
};
