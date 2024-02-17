import MessageBus from '$lib/bus/MessageBus';
import { Messages } from '$lib/bus/Messages';
import ConfigService from '$lib/services/ConfigService';

export interface ApiResponse<T> {
	errors?: string[];
	response?: T;
}

export default abstract class BaseApi {
	private async request<T>(url: string, request: RequestInit = {}): Promise<T> {
		let options: RequestInit = {
			method: 'GET',
			headers: {},
			...request
		};
		options.headers['Authorization'] = this.getBearerToken();

		let res = await fetch(this.getFullUrl(url), options);
		return (await res.json()) as T;
	}

	private getFullUrl(urlSuffix: string): string {
		if (process.env.NODE_ENV === 'test') return urlSuffix;

		let baseUrl = this.getBaseUrl();

		let baseUrlHasTrailingSlash = baseUrl.endsWith('/');
		let urlSuffixHasLeadingSlash = urlSuffix.startsWith('/');

		if (!baseUrlHasTrailingSlash && !urlSuffixHasLeadingSlash) return `${baseUrl}/${urlSuffix}`;

		if (baseUrlHasTrailingSlash && urlSuffixHasLeadingSlash)
			return `${baseUrl}${urlSuffix.substring(1)}`;

		return `${baseUrl}${urlSuffix}`;
	}

	private getBaseUrl(): string {
		let service = new ConfigService();

		return service.getConfig<string>('baseUrl');
	}

	private getBearerToken() {
		let token = MessageBus.getLastMessage<string>(Messages.UserToken);

		return `Bearer ${token}`;
	}

	private buildPostyRequestInit(body: any, method: string): RequestInit {
		let stringifedBody = this.stringifyBody(body);

		return {
			body: stringifedBody,
			method: method
		};
	}

	private stringifyBody(body: any) {
		if (typeof body === 'string') return body;

		return JSON.stringify(body);
	}

	protected async Get<T>(url: string): Promise<T> {
		return await this.request<T>(url);
	}

	protected async Post<T>(url: string, body: any): Promise<T> {
		return this.request<T>(url, this.buildPostyRequestInit(body, HTTP_METHODS.POST));
	}

	protected async Put<T>(url: string, body: any): Promise<T> {
		return this.request<T>(url, this.buildPostyRequestInit(body, HTTP_METHODS.PUT));
	}

	protected async Delete<T>(url: string): Promise<T> {
		return this.request<T>(url, { method: HTTP_METHODS.DELETE });
	}
}

const HTTP_METHODS = {
	GET: 'GET',
	POST: 'POST',
	PUT: 'PUT',
	DELETE: 'DELETE'
};