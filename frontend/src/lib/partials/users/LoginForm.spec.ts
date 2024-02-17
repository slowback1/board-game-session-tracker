import { act, fireEvent, render, type RenderResult } from '@testing-library/svelte';
import LoginForm from '$lib/partials/users/LoginForm.svelte';
import { beforeEach } from 'vitest';
import { SiteMap } from '$lib/utils/siteMap';

describe('LoginForm', () => {
	let result: RenderResult<LoginForm>;

	function renderComponent(
		props: Partial<{
			onSubmit: (username: string, password: string) => void;
			errors: string[];
		}> = {}
	) {
		if (result) result.unmount();

		result = render(LoginForm, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	it('contains a form', () => {
		expect(result.container.querySelector('form')).toBeInTheDocument();
	});

	it.each([
		['username', 'Username'],
		['password', 'Password']
	])('contains a field %s with label %s', (testId, label) => {
		let field = result.getByTestId(`login-form__${testId}`);

		expect(field).toBeInTheDocument();

		let labelElement = result.getByTestId(`login-form__${testId}-label`);
		expect(labelElement).toHaveTextContent(label);
	});

	it('the password field is of type password', () => {
		let passwordField = result.getByTestId('login-form__password') as HTMLInputElement;

		expect(passwordField.type).toEqual('password');
	});

	it('has a submit button', () => {
		let submitButton = result.getByTestId('login-form__submit');

		expect(submitButton).toBeInTheDocument();
	});

	async function fillOutUsernameAndPasswordFields() {
		let username = result.getByTestId('login-form__username');
		await act(() => {
			fireEvent.input(username, { target: { value: 'username' } });
		});
		expect(username).toHaveValue('username');

		let password = result.getByTestId('login-form__password');
		await act(() => {
			fireEvent.input(password, { target: { value: 'password' } });
		});
		expect(password).toHaveValue('password');
	}

	it('clicking the submit button calls the onSubmit prop', async () => {
		const onSubmitMock = vi.fn();

		renderComponent({ onSubmit: onSubmitMock });

		await fillOutUsernameAndPasswordFields();
		let submitButton = result.getByTestId('login-form__submit');

		fireEvent.click(submitButton);

		expect(onSubmitMock).toHaveBeenCalledWith('username', 'password');
	});

	it('rendering with errors displays an error summary', () => {
		renderComponent({ errors: ['test error'] });

		let errorSummary = result.getByTestId('error-summary');

		expect(errorSummary).toBeInTheDocument();
	});

	it('contains a link to the registration page', () => {
		let registerLink = result.getByTestId('login-form__register-link');

		expect(registerLink).toBeInTheDocument();
		expect(registerLink).toHaveAttribute('href', SiteMap.account.createUser());
	});
});
