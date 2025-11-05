<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>


				<Side>
					<xsl:value-of select="'Side'"/>
				</Side>

				<Symbol>
					<xsl:value-of select="'Symbol/Cusip'"/>
				</Symbol>


				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>


				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<CommissionType>
					<xsl:value-of select="'Commission Type'"/>
				</CommissionType>

				<COMMS>
					<xsl:value-of select="'Commission Value'"/>
				</COMMS>

				<MUD>
					<xsl:value-of select="'MUD'"/>
				</MUD>

				<SCT>
					<xsl:value-of select="'Sales Credit Type'"/>
				</SCT>

				<SCA>
					<xsl:value-of select="'Sales Credit Amount'"/>
				</SCA>

				<InvestmentManager>
					<xsl:value-of select="'Investment Manager'"/>
				</InvestmentManager>

				<Market>
					<xsl:value-of select="'Market'"/>
				</Market>

				<InstitutionalCommands>
					<xsl:value-of select="'Institutional Comments'"/>
				</InstitutionalCommands>

				<CreateAsStepIn>
					<xsl:value-of select="'Create as Step In'"/>
				</CreateAsStepIn>

				<Capacity>
					<xsl:value-of select="'Capacity'"/>
				</Capacity>
				
				
				<LegendCode1>
					<xsl:value-of select="'Legend Code 1'"/>
				</LegendCode1>

				<LegendCode2>
					<xsl:value-of select="'Legend Code 2'"/>
				</LegendCode2>

				<ClearanceOnly>
					<xsl:value-of select="'Clearance Only'"/>
				</ClearanceOnly>

				<ContraAccountID>
					<xsl:value-of select="'Contra Account ID'"/>
				</ContraAccountID>

				<DTCC>
					<xsl:value-of select="'DTCC Account #'"/>
				</DTCC>

				<AccountID>
					<xsl:value-of select="'Account Name'"/>
				</AccountID>

				<BIA>
					<xsl:value-of select="'BIA #'"/>
				</BIA>

				<SOConfirm>
					<xsl:value-of select="'S/0 Confirm'"/>
				</SOConfirm>

				<AccessCode>
					<xsl:value-of select="'Access Code'"/>
				</AccessCode>

				<BrokerofCredit>
					<xsl:value-of select="'Broker of Credit'"/>
				</BrokerofCredit>

				<RRInfo>
					<xsl:value-of select="'RR Info 1'"/>
				</RRInfo>

				<TimeofExecution>
					<xsl:value-of select="'Time of Execution'"/>
				</TimeofExecution>

				<Alert>
					<xsl:value-of select="'ALERT Acronym'"/>
				</Alert>

				<MISC>
					<xsl:value-of select="'MISC'"/>
				</MISC>

				<ExecutedBy>
					<xsl:value-of select="'Executed By'"/>
				</ExecutedBy>

				<Trailer>
					<xsl:value-of select="'Trailer'"/>
				</Trailer>

				<ShortSale>
					<xsl:value-of select="'Short Sale'"/>
				</ShortSale>

				<!--<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<CODE>
					<xsl:value-of select="'CODE'"/>
				</CODE>

				<UNITS>
					<xsl:value-of select="'UNITS'"/>
				</UNITS>

				<PRSHR>
					<xsl:value-of select="'PRSHR'"/>
				</PRSHR>

				<BROKER>
					<xsl:value-of select="'BROKER'"/>
				</BROKER>

				<TRADDT>
					<xsl:value-of select="'TRADDT'"/>
				</TRADDT>

				<CONTDT>
					<xsl:value-of select="'CONTDT'"/>
				</CONTDT>

				<COMMS>
					<xsl:value-of select="'COMMS'"/>
				</COMMS>

				<SECFees>
					<xsl:value-of select="'S.E.C. Fees'"/>
				</SECFees>

				<NET>
					<xsl:value-of select="'NET'"/>
				</NET>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITYDESCRIPTION'"/>
				</SECURITYDESCRIPTION>

				<PrincipalAmount>
					<xsl:value-of select ="'Principal Amount '"/>
				</PrincipalAmount>

				<AccountID>
					<xsl:value-of select ="'Account ID'"/>
				</AccountID>


				--><!-- system use only--><!--
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>-->

			</ThirdPartyFlatFileDetail>



			<xsl:for-each select="ThirdPartyFlatFileDetail">			
					<ThirdPartyFlatFileDetail>
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<xsl:variable name = "PRANA_FUND_NAME">
							<xsl:value-of select="AccountName"/>
						</xsl:variable>

						<xsl:variable name ="PB_FUND_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name='Lyrical']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="Side='Buy to Open' or Side='Buy' ">
									
										<xsl:value-of select ="'B'"/>
									
								</xsl:when>
								<xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
									
										<xsl:value-of select ="'CS'"/>
									
								</xsl:when>
								
								<xsl:when test="Side='Sell' or Side='Sell to Close' ">
									
										<xsl:value-of select ="'S'"/>
									
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
									
										<xsl:value-of select ="'SS'"/>
									
								</xsl:when>
								<xsl:otherwise>
									
										<xsl:value-of select="Side"/>
									
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<Symbol>
							<xsl:value-of select="Symbol"/>
						</Symbol>


						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>

						<Price>
							<xsl:value-of select="AveragePrice"/>
						</Price>

						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="SettlementDate"/>
						</SettlementDate>

						<CommissionType>
							<xsl:value-of select="'COM'"/>
						</CommissionType>

						<COMMS>
							<xsl:value-of select="CommissionCharged"/>
						</COMMS>
						
						<MUD>
							<xsl:value-of select="''"/>
						</MUD>

						<SCT>
							<xsl:value-of select="''"/>
						</SCT>

						<SCA>
							<xsl:value-of select="''"/>
						</SCA>

						<InvestmentManager>
							<xsl:value-of select="'LYRICAL'"/>
						</InvestmentManager>

						<Market>
							<xsl:value-of select="'OTC'"/>
						</Market>

						<InstitutionalCommands>
							<xsl:value-of select="''"/>
						</InstitutionalCommands>

						<CreateAsStepIn>
							<xsl:value-of select="'YES'"/>
						</CreateAsStepIn>
						
						<Capacity>
					<xsl:value-of select="''"/>
				</Capacity>

						<LegendCode1>
							<xsl:value-of select="''"/>
						</LegendCode1>

						<LegendCode2>
							<xsl:value-of select="''"/>
						</LegendCode2>

						<ClearanceOnly>
							<xsl:value-of select="'YES'"/>
						</ClearanceOnly>

						<ContraAccountID>
							<xsl:value-of select="''"/>
						</ContraAccountID>

						<DTCC>
							<xsl:choose>
								<xsl:when test="CounterParty='JPM' ">
							<xsl:value-of select="'JPMC'"/>
								</xsl:when>
								<xsl:otherwise>

									<xsl:value-of select="CounterParty"/>

								</xsl:otherwise>
							</xsl:choose>


						</DTCC>

						<AccountID>
							<xsl:value-of select="''"/>
						</AccountID>
						
						

						<BIA>

							<xsl:choose>
								<xsl:when test="AccountName='Conor Bastable: RZF001030' ">

									<xsl:value-of select ="'RZF001030'"/>

								</xsl:when>


								<xsl:when test="AccountName='Aspen Partners LLC: RZE003003' ">

									<xsl:value-of select ="'RZE003003'"/>

								</xsl:when>
								<xsl:when test="AccountName='Adam Friedman Trust : QXV003240'">
									<xsl:value-of select ="'QXV003240'"/>
								</xsl:when>
								<xsl:when test="AccountName='Ana Meier: QXV002804'">
									<xsl:value-of select ="'QXV002804'"/>
								</xsl:when>
								<xsl:when test="AccountName='Anita Friedman Trust: QXV003216'">
									<xsl:value-of select ="'QXV003216'"/>
								</xsl:when>
								<xsl:when test="AccountName='Aspen Partners LLC: RZE003003'">
									<xsl:value-of select ="'RZE003003'"/>
								</xsl:when>
								<xsl:when test="AccountName='Axilrod Children&quot;s Trust: QXV002788'">
									<xsl:value-of select ="'QXV002788'"/>
								</xsl:when>
								<xsl:when test="AccountName='Carrol Saunders Roberson: RZE001676'">
									<xsl:value-of select ="'RZE001676'"/>
								</xsl:when>
								<xsl:when test="AccountName='Chapin School: RZF001238'">
									<xsl:value-of select ="'RZF001238'"/>
								</xsl:when>
								<xsl:when test="AccountName='Christian Education Foundation: RZF001071'">
									<xsl:value-of select ="'RZF001071'"/>
								</xsl:when>
								<xsl:when test="AccountName='Clara Y. Bingham: QXV002549'">
									<xsl:value-of select ="'QXV002549'"/>
								</xsl:when>
								<xsl:when test="AccountName='Clevedon Trust : QXV003125'">
									<xsl:value-of select ="'QXV003125'"/>
								</xsl:when>
								<xsl:when test="AccountName='Conor Bastable: RZF001030'">
									<xsl:value-of select ="'RZF001030'"/>
								</xsl:when>
								<xsl:when test="AccountName='Craig Blase: RZE001569'">
									<xsl:value-of select ="'RZE001569'"/>
								</xsl:when>
								<xsl:when test="AccountName='Daniel Schapiro IRA Rollover: QXV002895'">
									<xsl:value-of select ="'QXV002895'"/>
								</xsl:when>
								<xsl:when test="AccountName='David Coleman: QXV003109'">
									<xsl:value-of select ="'QXV-003109'"/>
								</xsl:when>
								<xsl:when test="AccountName='Dogwood Enterprises: QXV001384'">
									<xsl:value-of select ="'QXV-001384'"/>
								</xsl:when>
								<xsl:when test="AccountName='Four Winds Capital LP: RZE002765'">
									<xsl:value-of select ="'RZE002765'"/>
								</xsl:when>
								<xsl:when test="AccountName='Hugh and Charlotte MacLellan: RZF001063'">
									<xsl:value-of select ="'RZF001063'"/>
								</xsl:when>
								<xsl:when test="AccountName='Jane Ehrenkranz: QXV003232'">
									<xsl:value-of select ="'QXV003232'"/>
								</xsl:when>
								<xsl:when test="AccountName='Jeffrey Katz 2010 Family Trust: QXV002432'">
									<xsl:value-of select ="'QXV002432'"/>
								</xsl:when>
								<xsl:when test="AccountName='Jeffrey Katz: QXV001723'">
									<xsl:value-of select ="'QXV001723'"/>
								</xsl:when>
								<xsl:when test="AccountName='Joel S. Ehrenkranz: QXV003091'">
									<xsl:value-of select ="'QXV003091'"/>
								</xsl:when>
								<xsl:when test="AccountName='Johnson Family GST Trust: QXV003166'">
									<xsl:value-of select ="'QXV003166'"/>
								</xsl:when>
								<xsl:when test="AccountName='Jonathan Friedman  Trust: QXV003257'">
									<xsl:value-of select ="'QXV003257'"/>
								</xsl:when>
								<xsl:when test="AccountName='Joseph and Cheryl JTIC: RZF001113'">
									<xsl:value-of select ="'RZF001113'"/>
								</xsl:when>
								<xsl:when test="AccountName='Joseph L. Gitterman III Revocable Trust: QXV002705'">
									<xsl:value-of select ="'QXV002705'"/>
								</xsl:when>
								<xsl:when test="AccountName='Kresko Holdings LP: QXV003372'">
									<xsl:value-of select ="'QXV003372'"/>
								</xsl:when>
								<xsl:when test="AccountName='Lamar Legacy LP: RZE002435'">
									<xsl:value-of select ="'RZE002435'"/>
								</xsl:when>
								<xsl:when test="AccountName='Lee Manning Vogelstein: QXV003307'">
									<xsl:value-of select ="'QXV003307'"/>
								</xsl:when>
								<xsl:when test="AccountName='Marianne Schaprio Roth IRA: QXV002911'">
									<xsl:value-of select ="'QXV002911'"/>
								</xsl:when>
								<xsl:when test="AccountName='Paul Edelman: QXV002812'">
									<xsl:value-of select ="'QXV002812'"/>
								</xsl:when>
								<xsl:when test="AccountName='RDB Family Trust: RZE002203'">
									<xsl:value-of select ="'RZE002203'"/>
								</xsl:when>
								<xsl:when test="AccountName='Richard Axilrod: QXV003174'">
									<xsl:value-of select ="'QXV003174'"/>
								</xsl:when>
								<xsl:when test="AccountName='Richard T. Silver: QXV002937'">
									<xsl:value-of select ="'QXV002937'"/>
								</xsl:when>
								<xsl:when test="AccountName='Robert DeNiro Rollover: QXV003349'">
									<xsl:value-of select ="'QXV003349'"/>
								</xsl:when>
								<xsl:when test="AccountName='Robert and Kathrina Foundation: RZF001246'">
									<xsl:value-of select ="'RZF001246'"/>
								</xsl:when>
								<xsl:when test="AccountName='Roger C. Altman: QXV002556'">
									<xsl:value-of select ="'QXV002556'"/>
								</xsl:when>
								<xsl:when test="AccountName='Ronald and Barbara Balser: RZE002229'">
									<xsl:value-of select ="'RZE002229'"/>
								</xsl:when>
								<xsl:when test="AccountName='Ronald Balser Trust: RZE002211'">
									<xsl:value-of select ="'RZE002211'"/>
								</xsl:when>
								<xsl:when test="AccountName='Silverman Family Investors LLC: QXV002564'">
									<xsl:value-of select ="'QXV002564'"/>
								</xsl:when>
								<xsl:when test="AccountName='Smith 1996 Family Trust No. 1: RZF001089'">
									<xsl:value-of select ="'RZF001089'"/>
								</xsl:when>
								<xsl:when test="AccountName='Socatean Partners: QXV003000'">
									<xsl:value-of select ="'QXV003000'"/>
								</xsl:when>
								<xsl:when test="AccountName='Taylor 1998 Irrevocable Trust: QXV002861'">
									<xsl:value-of select ="'QXV002861'"/>
								</xsl:when>
								<xsl:when test="AccountName='The John Katz Investment Trust 1: QXV002713'">
									<xsl:value-of select ="'QXV002713'"/>
								</xsl:when>
								<xsl:when test="AccountName='The John Katz Lifetime Trust: QXV002721'">
									<xsl:value-of select ="'QXV002721'"/>
								</xsl:when>
								<xsl:when test="AccountName='The MacLellan Foundation Inc: RZF001048'">
									<xsl:value-of select ="'RZF001048'"/>
								</xsl:when>
								<xsl:when test="AccountName='Westwood 1998 Irrevocable Trust: QXV002853'">
									<xsl:value-of select ="'QXV002853'"/>
								</xsl:when>



								<xsl:otherwise>

									<xsl:value-of select="''"/>

								</xsl:otherwise>
							</xsl:choose>
							
							<xsl:value-of select="''"/>
						</BIA>

						<SOConfirm>
							<xsl:value-of select="''"/>
						</SOConfirm>

						<AccessCode>
							<xsl:value-of select="''"/>
						</AccessCode>

						<BrokerofCredit>
							<xsl:value-of select="''"/>
						</BrokerofCredit>

						<RRInfo>
							<xsl:value-of select="''"/>
						</RRInfo>

					<TimeofExecution>
						<xsl:value-of select="''"/>
					</TimeofExecution>

						<Alert>
							<xsl:value-of select="''"/>
						</Alert>

						<MISC>
							<xsl:value-of select="''"/>
						</MISC>

						<ExecutedBy>
							<xsl:value-of select="''"/>
						</ExecutedBy>

					<Trailer>
						<xsl:value-of select="''"/>
					</Trailer>

					<ShortSale>
						<xsl:value-of select="''"/>
					</ShortSale>


						<!--<CUSIP>
							<xsl:value-of select="concat(concat('=&quot;',CUSIP),'&quot;')"/>
						</CUSIP>

						<CODE>
							<xsl:value-of select="translate(Side,$varSmall,$varCapital)"/>
						</CODE>

						<UNITS>
							<xsl:value-of select="AllocatedQty"/>
						</UNITS>

						<PRSHR>
							<xsl:value-of select="AveragePrice"/>
						</PRSHR>

						<BROKER>
							<xsl:value-of select="CounterParty"/>
						</BROKER>

						<TRADDT>
							<xsl:value-of select="concat(substring(TradeDate,1,6),substring(TradeDate,9,2))"/>
						</TRADDT>

						<CONTDT>
							<xsl:value-of select="concat(substring(SettlementDate,1,6),substring(SettlementDate,9,2))"/>
						</CONTDT>

						<COMMS>
							<xsl:value-of select="CommissionCharged"/>
						</COMMS>

						<SECFees>
							<xsl:value-of select="StampDuty"/>
						</SECFees>

						<NET>
							<xsl:value-of select="NetAmount"/>
						</NET>

						<TICKER>
							<xsl:value-of select="Symbol"/>
						</TICKER>

						<SECURITYDESCRIPTION>
							<xsl:value-of select="FullSecurityName"/>
						</SECURITYDESCRIPTION>

						<PrincipalAmount>
							<xsl:value-of select ="GrossAmount"/>
						</PrincipalAmount>

						<AccountID>
							<xsl:choose>
						

								<xsl:when test="FundName ='LYRIX-000000000000940'">
									<xsl:value-of select ="'TMS - 93802322'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountID>
						

						--><!-- system use only--><!--
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>-->
					</ThirdPartyFlatFileDetail>
		
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
