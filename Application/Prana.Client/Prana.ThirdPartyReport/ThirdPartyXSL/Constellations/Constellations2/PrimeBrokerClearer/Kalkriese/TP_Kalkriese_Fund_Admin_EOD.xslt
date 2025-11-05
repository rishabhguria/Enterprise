<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Kalkriese Capital Management']">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<portfolio>
						<xsl:value-of select="'kalk'"/>
					</portfolio>

					<trancode>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'by'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'sl'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short'">
								<xsl:value-of select="'ss'"/>
							</xsl:when>

							<xsl:when test="Side='Cover Short' or Side='Buy to Close' or Side='Buy minus'">
								<xsl:value-of select="'cs'"/>
							</xsl:when>
						</xsl:choose>
					</trancode>

					<comment>
						<xsl:value-of select="''"/>
					</comment>

					<!--<type1>
						<xsl:value-of select="''"/>
					</type1>-->

					<type1>
						<xsl:value-of select="concat('cs',translate(substring(CurrencySymbol,1,2),$varCapital,$varSmall))"/>
					</type1>

					<symbol1>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="translate(OSIOptionSymbol,$varCapital,$varSmall)"/>
							</xsl:when>
							<xsl:when test ="CurrencySymbol !='USD'">
								<xsl:choose>

									<xsl:when test="BBCode='' or BBCode=''">
										<xsl:value-of select ="translate(concat(substring-before(Symbol,'-'),' ',substring(CurrencySymbol,1,2)),$varCapital,$varSmall)"/>
									</xsl:when>
									<xsl:when test="contains(BBCode,'EQUITY')">
										<xsl:value-of select="translate(substring-before(BBCode,'EQUITY'),$varCapital,$varSmall)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="translate(BBCode,$varCapital,$varSmall)"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="translate(Symbol,$varCapital,$varSmall)"/>
							</xsl:otherwise>
						</xsl:choose>
					</symbol1>

					<trade>
						<xsl:value-of select="concat(substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'),substring-after(substring-after(TradeDate,'/'),'/'))"/>
					</trade>

					<settle>
						<xsl:value-of select="concat(substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'),substring-after(substring-after(SettlementDate,'/'),'/'))"/>
					</settle>

					<taxdate>
						<xsl:value-of select="''"/>
					</taxdate>

					<qty>
						<xsl:value-of select="AllocatedQty"/>
					</qty>

					<cmeth>
						<xsl:value-of select="''"/>
					</cmeth>

					<versus>
						<xsl:value-of select="''"/>
					</versus>

					<type2>
						<xsl:value-of select="concat('ca',translate(substring(CurrencySymbol,1,2),$varCapital,$varSmall))"/>
					</type2>

					<symbol2>
						<xsl:value-of select="'cash'"/>
					</symbol2>

					<tdfx>
						<xsl:value-of select="FXRate_Taxlot"/>
					</tdfx>

					<sdfx>
						<!--<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>

							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'1'"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="FXRate_Taxlot"/>
					</sdfx>

					<taxfx>
						<xsl:value-of select="''"/>
					</taxfx>

					<mtm>
						<!--<xsl:value-of select="''"/>-->
						<xsl:choose>
							<xsl:when test="CurrencySymbol!='USD'">
								<xsl:value-of select="'y'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</mtm>

					<amount>
						<xsl:value-of select="format-number(NetAmount,'0.##')"/>
					</amount>

					<taxcost>
						<xsl:value-of select="AveragePrice"/>
					</taxcost>

					<reserved>
						<xsl:value-of select="''"/>
					</reserved>

					<fwtax>
						<xsl:value-of select="''"/>
					</fwtax>

					<exchange>
						<xsl:value-of select="''"/>
					</exchange>

					<secfee>
						<xsl:value-of select="SecFee"/>
					</secfee>

					<commis>
						<xsl:value-of select="CommissionCharged"/>
					</commis>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="PB_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='NT']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<broker>
						<xsl:choose>
							<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</broker>

					<implied>
						<xsl:value-of select="'n'"/>
					</implied>

					<other>
						<xsl:value-of select="OtherBrokerFee"/>
					</other>

					<compurp>
						<xsl:value-of select="''"/>
					</compurp>

					<pledge>
						<xsl:value-of select="'n'"/>
					</pledge>

					<location>
						<xsl:value-of select="'254'"/>
					</location>

					<dstpledge>
						<xsl:value-of select="''"/>
					</dstpledge>

					<dstloc>
						<xsl:value-of select="''"/>
					</dstloc>

					<origface>
						<xsl:value-of select="''"/>
					</origface>

					<origyld>
						<xsl:value-of select="''"/>
					</origyld>

					<origdur>
						<xsl:value-of select="''"/>
					</origdur>


					<userdef1>
						<xsl:value-of select="''"/>
					</userdef1>

					<userdef2>
						<xsl:value-of select="''"/>
					</userdef2>

					<userdef3>
						<xsl:value-of select="''"/>
					</userdef3>

					<tranid>
						<xsl:value-of select="''"/>
					</tranid>

					<ipctr>
						<xsl:value-of select="''"/>
					</ipctr>

					<replace>
						<xsl:value-of select="''"/>
					</replace>

					<source>
						<xsl:value-of select="''"/>
					</source>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
