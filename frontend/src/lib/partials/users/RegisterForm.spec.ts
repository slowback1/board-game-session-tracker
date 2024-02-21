import { act, fireEvent, render, type RenderResult } from '@testing-library/svelte';
import RegisterForm from '$lib/partials/users/RegisterForm.svelte';
import type { CreateUserRequest } from '$lib/api/userApi';
import { beforeEach, expect } from 'vitest';

describe('RegisterForm', () => {
	let result: RenderResult<RegisterForm>;

	function renderComponent(
		props: Partial<{
			onSubmit: (createRequest: CreateUserRequest) => void;
			errors: string[];
		}> = {}
	) {
		if (result) result.unmount();

		result = render(RegisterForm, { props });
	}

	beforeEach(() => {
		renderComponent();
	});

	it('contains a form', () => {
		expect(result.container.querySelector('form')).toBeInTheDocument();
	});

	it('contains a page title', () => {
		let pageTitle = result.getByTestId('page-title');

		expect(pageTitle).toBeInTheDocument();
		expect(pageTitle).toHaveTextContent('Register User');
	});

	it.each([
		['username', 'Username'],
		['password', 'Password'],
		['confirmPassword', 'Confirm Password']
	])('contains a field %s with label %s', (testId, label) => {
		let field = result.getByTestId(`register-form__${testId}`);

		expect(field).toBeInTheDocument();

		let labelElement = result.getByTestId(`register-form__${testId}-label`);
		expect(labelElement).toHaveTextContent(label);
	});

	it.each(['password', 'confirmPassword'])('the %s field is of type password', (field) => {
		let passwordField = result.getByTestId(`register-form__${field}`) as HTMLInputElement;

		expect(passwordField.type).toEqual('password');
	});

	it('has a submit button', () => {
		let submitButton = result.getByTestId('register-form__submit');

		expect(submitButton).toBeInTheDocument();
	});

	async function fillOutUsernameAndPasswordFields() {
		let username = result.getByTestId('register-form__username');
		await act(() => {
			fireEvent.input(username, { target: { value: 'username' } });
		});
		expect(username).toHaveValue('username');

		let password = result.getByTestId('register-form__password');
		await act(() => {
			fireEvent.input(password, { target: { value: 'password' } });
		});
		expect(password).toHaveValue('password');

		let confirmPassword = result.getByTestId('register-form__confirmPassword');
		await act(() => {
			fireEvent.input(confirmPassword, { target: { value: 'confirmPassword' } });
		});
		expect(confirmPassword).toHaveValue('confirmPassword');
	}

	it('clicking the submit button calls the onSubmit prop', async () => {
		const onSubmitMock = vi.fn();

		renderComponent({ onSubmit: onSubmitMock });

		await fillOutUsernameAndPasswordFields();
		let submitButton = result.getByTestId('register-form__submit');

		fireEvent.click(submitButton);

		expect(onSubmitMock).toHaveBeenCalledWith({
			username: 'username',
			password: 'password',
			confirmPassword: 'confirmPassword'
		});
	});

	it('rendering with errors displays an error summary', () => {
		renderComponent({ errors: ['test error'] });

		let errorSummary = result.getByTestId('error-summary');

		expect(errorSummary).toBeInTheDocument();
	});
});
