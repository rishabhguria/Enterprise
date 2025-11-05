<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				
				<xsl:variable name="varDRAmount">
				       <xsl:call-template name="Translate">
				       	<xsl:with-param name="Number" select="normalize-space(COL6)"/>
				       </xsl:call-template>
				 </xsl:variable>

				<xsl:variable name="varCRAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL7)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varLedgerAccount">
					<xsl:value-of select="normalize-space(COL2)"/>
				</xsl:variable>


				<xsl:if test="($varLedgerAccount ='INVESTMENT INTEREST RECEIVABLE' or $varLedgerAccount='EXPENSE REIMBURSEMENT RECEIVABLE' or $varLedgerAccount='12B 1 FEES' or 
                                                         $varLedgerAccount='ADMINISTRATION EXPENSE' or $varLedgerAccount='ADVISOR FEES' or $varLedgerAccount='ADVISOR FEES WAIVER'or $varLedgerAccount='AUDIT EXPENSE' or 
                                                         $varLedgerAccount='BLUE SKY EXPENSE' or $varLedgerAccount='CUSTODY EXPENSE' or $varLedgerAccount='DIRECTORS EXPENSE' or
                                                         $varLedgerAccount='INSURANCE EXPENSE' or$varLedgerAccount='LEGAL EXPENSE' or $varLedgerAccount='PRINTING AND MAILING EXPENSE' or 
                                                         $varLedgerAccount='REGISTRATION EXPENSE' or $varLedgerAccount='TRANSFER AGENT FEES' or $varLedgerAccount='ADVISOR FEES WAIVER' or 
                                                         $varLedgerAccount ='INTEREST EXPENSE CREDIT LINE' or $varLedgerAccount ='INVESTMENT INTEREST RECEIVABLE' or $varLedgerAccount='ACCOUNTING EXPENSE'
                                                          or $varLedgerAccount='EXPENSE REIMBURSEMENT' or $varLedgerAccount='MISCELLANEOUS EXPENSE'
                                                         )">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<!--<xsl:variable name="varCash">
							<xsl:choose>
								<xsl:when test="$varDRAmount  &gt; 0">
									<xsl:value-of select="$varDRAmount"/>
								</xsl:when>
								<xsl:when test="$varDRAmount &lt; 0">
									<xsl:value-of select="$varDRAmount*(-1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>-->

						<xsl:variable name = "PRANA_PRE_ACRONYM_NAME" >
							<xsl:choose>
								<xsl:when test="$varLedgerAccount ='INVESTMENT INTEREST RECEIVABLE'">
									<xsl:value-of select="'INTREC'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='EXPENSE REIMBURSEMENT RECEIVABLE'">
									<xsl:value-of select="'ReimburseReceivable'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='12B 1 FEES'">
									<xsl:value-of select="'12B 1 Fees'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ACCOUNTING EXPENSE'">
									<xsl:value-of select="'Accounting Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ADMINISTRATION EXPENSE'">
									<xsl:value-of select="'Administration Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ADVISOR FEES'">
									<xsl:value-of select="'Advisor Fees'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ADVISOR FEES WAIVER'">
									<xsl:value-of select="'Advisor Fees Waiver'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='AUDIT EXPENSE'">
									<xsl:value-of select="'Audit Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='CUSTODY EXPENSE'">
									<xsl:value-of select="'Custody Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='DIRECTORS EXPENSE'">
									<xsl:value-of select="'Directors Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='INSURANCE EXPENSE'">
									<xsl:value-of select="'INS EXP'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='LEGAL EXPENSE'">
									<xsl:value-of select="'Legal Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='PRINTING AND MAILING EXPENSE'">
									<xsl:value-of select="'Printing And Mailing Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='REGISTRATION EXPENSE'">
									<xsl:value-of select="'Registration Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='TRANSFER AGENT FEES'">
									<xsl:value-of select="'Transfer Agent Fees'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='BLUE SKY EXPENSE'">
									<xsl:value-of select="'Blue Sky Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='INTEREST EXPENSE CREDIT LINE'">
									<xsl:value-of select="'Interest Expense Credit Line'"/>
								</xsl:when>					
								<xsl:when test="$varLedgerAccount='MISCELLANEOUS EXPENSE'">
									<xsl:value-of select="'MISEXP'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "PRANA_Post_ACRONYM_NAME" >
							<xsl:choose>
								<xsl:when test="$varLedgerAccount ='INVESTMENT INTEREST RECEIVABLE'">
									<xsl:value-of select="'Intincome'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='EXPENSE REIMBURSEMENT RECEIVABLE'">
									<xsl:value-of select="'Expense Reimbursement'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='12B 1 FEES'">
									<xsl:value-of select="'Accrued 12B 1 Fees'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ACCOUNTING EXPENSE'">
									<xsl:value-of select="'Accrued Accounting Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ADMINISTRATION EXPENSE'">
									<xsl:value-of select="'Accrued Administration Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ADVISOR FEES'">
									<xsl:value-of select="'Accrued Advisor Fees'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='ADVISOR FEES WAIVER'">
									<xsl:value-of select="'Accured Advisor Fees Waiver'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='AUDIT EXPENSE'">
									<xsl:value-of select="'Accrued Audit Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='CUSTODY EXPENSE'">
									<xsl:value-of select="'Accrued Custody Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='DIRECTORS EXPENSE'">
									<xsl:value-of select="'Accrued Directors Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='INSURANCE EXPENSE'">
									<xsl:value-of select="'Accrued Insurance Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='LEGAL EXPENSE'">
									<xsl:value-of select="'Accrued Legal Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='PRINTING AND MAILING EXPENSE'">
									<xsl:value-of select="'Accrued Printing And Mailing Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='REGISTRATION EXPENSE'">
									<xsl:value-of select="'Accrued Registration Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='TRANSFER AGENT FEES'">
									<xsl:value-of select="'Accrued Transfer Agent Fees'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='BLUE SKY EXPENSE'">
									<xsl:value-of select="'Accrued Blue Sky Expense'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='INTEREST EXPENSE CREDIT LINE'">
									<xsl:value-of select="'Accrued Interest Expense Credit Line'"/>
								</xsl:when>
								<xsl:when test="$varLedgerAccount='MISCELLANEOUS EXPENSE'">
									<xsl:value-of select="'Accrued Misc Expense'"/>
								</xsl:when>						
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
										       			       				       
						<JournalEntries>
							<xsl:choose>							
								<xsl:when test="$varDRAmount &gt;0 ">
									<xsl:value-of select="concat($PRANA_PRE_ACRONYM_NAME, ':' , $varDRAmount , '|' , $PRANA_Post_ACRONYM_NAME, ':' , $varDRAmount)"/>
								</xsl:when>
								<xsl:when test="$varDRAmount &lt; 0 or $varDRAmount = 0">
									<xsl:value-of select="concat($PRANA_Post_ACRONYM_NAME, ':' , $varCRAmount , '|' , $PRANA_PRE_ACRONYM_NAME, ':' , $varCRAmount)"/>
								</xsl:when>															
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name="varDescription">
							<xsl:value-of select="normalize-space(COL10)" />
						</xsl:variable>

						<Description>
							<xsl:choose>
								<xsl:when test="$varDescription!=''">
									<xsl:value-of select="$varDescription" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''" />
								</xsl:otherwise>
							</xsl:choose>
						</Description>

						<xsl:variable name = "varTradeDate" >
							<xsl:value-of select="''"/>
						</xsl:variable>

						<Date>
							<xsl:value-of select="$varTradeDate"/>
						</Date>

						<!--<CurrencyName>
							<xsl:value-of select="COL12"/>
						</CurrencyName>-->

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>