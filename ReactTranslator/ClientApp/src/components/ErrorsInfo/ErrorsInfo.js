import React from 'react';

const ErrorsInfo = ({ errors }) => {
    return (
        <div className="alert alert-danger" role="alert">
            {errors}
        </div>
    );
};
export default ErrorsInfo;