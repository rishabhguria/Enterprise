<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<FileHeader>
					<xsl:value-of select="'true'"/>
				</FileHeader>
				
				<CUSIP>
					<xsl:value-of select="concat('&#x0A;&#x0A;','CUSIP')"/>
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

				<S.E.C.Fees>
					<xsl:value-of select="'S.E.C.Fees'"/>
				</S.E.C.Fees>

				<NET>
					<xsl:value-of select="'NET'"/>
				</NET>

				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<SECURITYDESCRIPTION>
					<xsl:value-of select="'SECURITY DESCRIPTION'"/>
				</SECURITYDESCRIPTION>
				

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>


			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>
					
					<CUSIP>
						<xsl:choose>
							<xsl:when test ="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CUSIP>

					<CODE>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>

							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CODE>

					<UNITS>
						<xsl:choose>
							<xsl:when test ="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</UNITS>

					<PRSHR>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="format-number(AveragePrice,'####.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</PRSHR>
					
					<xsl:variable name="PB_NAME" select="'Statestreet'"/>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<BROKER>
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'JPMorgan Securities LLC'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'Merrill Lynch Broadcort'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'Sanders Morris Harris LLC'"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

						<!--<xsl:value-of select="$Broker"/>-->
					</BROKER>
					

					<TRADDT>
						<xsl:value-of select="TradeDate"/>
					</TRADDT>

					<CONTDT>
						<xsl:value-of select="SettlementDate"/>
					</CONTDT>
					
					<xsl:variable name="varCommission" select="CommissionCharged + SoftCommissionCharged"/>
					<COMMS>
						<xsl:value-of select="format-number($varCommission,'##.##')"/>
					</COMMS>

					

					<S.E.C.Fees>
						<xsl:choose>
							<xsl:when test="number(StampDuty)">
								<xsl:value-of select="format-number(StampDuty,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</S.E.C.Fees>

					<NET>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="format-number(NetAmount,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NET>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<TICKER>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TICKER>

					<SECURITYDESCRIPTION>
						<xsl:value-of select="FullSecurityName"/>
					</SECURITYDESCRIPTION>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				
			</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>