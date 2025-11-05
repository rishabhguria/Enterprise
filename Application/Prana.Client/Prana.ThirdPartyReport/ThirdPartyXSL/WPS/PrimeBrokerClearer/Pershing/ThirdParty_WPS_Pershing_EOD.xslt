<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="GetMonth">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 1" >
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month = 2" >
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month = 3" >
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month = 4" >
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month = 5" >
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month = 6" >
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month = 7" >
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month = 8" >
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month = 9" >
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month = 10" >
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month = 11" >
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month = 12" >
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Count">
		<xsl:param name="Symbol"/>
		<xsl:value-of select="count(//ThirdPartyFlatFileDetail[Symbol=$Symbol])"/>
	</xsl:template>

	<xsl:template name="SumPrice">
		<xsl:param name="Symbol"/>
		<xsl:value-of select="sum(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/AveragePrice)"/>
	</xsl:template>

	<xsl:template name="BLK">
		<xsl:param name="ID"/>
		<xsl:param name="Symbol"/>
		<xsl:choose>
			<xsl:when test="$ID = (//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)">
				<xsl:value-of select="(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			
			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty = 'PERS']">
				
				<ThirdPartyFlatFileDetail>

					<xsl:variable name="Count">
						<xsl:call-template name="Count">
							<xsl:with-param name="Symbol" select="Symbol"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="SumPrice">
						<xsl:call-template name ="SumPrice">
							<xsl:with-param name="Symbol" select="Symbol"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="AvgofPrice">
						<xsl:value-of select="AveragePrice"/>
					</xsl:variable>

					<xsl:variable name="BLK">
						<xsl:call-template name="BLK">
							<xsl:with-param name="ID" select="PBUniqueID"/>
							<xsl:with-param name="Symbol" select="Symbol"/>
						</xsl:call-template>
					</xsl:variable>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'true'"/>
					</FileFooter>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="HH">
						<xsl:choose>
							<xsl:when test="string-length(substring-before(substring-after(TradeDateTime,' '),':'))=1">
								<xsl:value-of select="concat('0',substring-before(substring-after(TradeDateTime,' '),':'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-before(substring-after(TradeDateTime,' '),':')"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<xsl:variable name="MM">
						<xsl:choose>
							<xsl:when test="string-length(substring-before(substring-after(substring-after(TradeDateTime,' '),':'),':'))=1">
								<xsl:value-of select="concat('0',substring-before(substring-after(substring-after(TradeDateTime,' '),':'),':'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-before(substring-after(substring-after(TradeDateTime,' '),':'),':')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="SS">
						<xsl:choose>
							<xsl:when test="string-length(substring-before(substring-after(substring-after(substring-after(TradeDateTime,' '),':'),':'),' '))=1">
								<xsl:value-of select="concat('0',substring-before(substring-after(substring-after(substring-after(TradeDateTime,' '),':'),':'),' '))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(TradeDateTime,' '),':'),':'),' ')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="AMPM">
						<xsl:value-of select="substring(substring-after(substring-after(TradeDateTime,' '),' '),1,1)"/>
					</xsl:variable>

					<LOCALREF>
						<xsl:value-of select="concat('T8A',substring(substring-before(TradeDate,'/'),2,1),substring(PBUniqueID,string-length(PBUniqueID)-3),substring(CompanyAccountID,string-length(CompanyAccountID)-1))"/>
					</LOCALREF>

					<CFID>
						<xsl:value-of select="concat('T8A',substring(substring-before(TradeDate,'/'),2,1),substring(PBUniqueID,string-length(PBUniqueID)-3),substring(CompanyAccountID,string-length(CompanyAccountID)-1))"/>
					</CFID>

					<ROUTECD>
						<xsl:value-of select="'PSHG'"/>
					</ROUTECD>

					<TIRORDERID>
						<xsl:value-of select="concat('BLK', $BLK)"/>
					</TIRORDERID>

					<TIRPIECE>
						<xsl:value-of select="''"/>
					</TIRPIECE>

					<TIRSEQ>
						<xsl:value-of select="''"/>
					</TIRSEQ>

					<SECIDTYPE>
						<xsl:choose>
							<xsl:when test="contains(Symbol,'-')">
								<xsl:value-of select="'I'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="'C'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'S'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</SECIDTYPE>

					<SECURITYID>
						<xsl:choose>
							<xsl:when test="contains(Symbol,'-')">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="CUSIP"/>-->
					</SECURITYID>

					<xsl:variable name="varCounterParty">
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>
					

					<DESCRIPTION1>
						<xsl:choose>
							<xsl:when test="$varCounterParty = 'JPGA'">
								<xsl:value-of select="'JPMS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</DESCRIPTION1>

					<DESCRIPTION2>
						<xsl:value-of select="''"/>
					</DESCRIPTION2>

					<DESCRIPTION3>
						<xsl:value-of select="''"/>
					</DESCRIPTION3>

					<DESCRIPTION4>
						<xsl:value-of select="''"/>
					</DESCRIPTION4>

					<xsl:variable name="varTradeMonth">
						<xsl:call-template name="GetMonth">
							<xsl:with-param name="Month" select="substring(TradeDate,1,2)"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varSettleMonth">
						<xsl:call-template name="GetMonth">
							<xsl:with-param name="Month" select="substring(SettlementDate,1,2)"/>
						</xsl:call-template>
					</xsl:variable>

					<TRADEDATE>
						<xsl:value-of select="concat(substring(TradeDate,4,2),'-',$varTradeMonth,'-',substring(TradeDate,9))"/>
					</TRADEDATE>

					<SETLDATE>
						<xsl:value-of select="concat(substring(SettlementDate,4,2),'-',$varSettleMonth,'-',substring(SettlementDate,9))"/>
					</SETLDATE>

					<QUANTITY>
						<xsl:choose>
							<xsl:when test="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</QUANTITY>

					<QUANTITYDESC>
						<xsl:value-of select="''"/>
					</QUANTITYDESC>

					<NETMONEY>
						<xsl:value-of select="''"/>
					</NETMONEY>

					<CASHACCOUNT>
						<xsl:value-of select="'T8A8977782'"/>
					</CASHACCOUNT>

					<SECACCOUNT>
					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Close'">							
								<xsl:value-of select="concat(AccountNo,'1')"/>							
						</xsl:when>
						
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open'">							
								<xsl:value-of select="concat(AccountNo,'3')"/>
						</xsl:when>
						
						<xsl:otherwise>							
								<xsl:value-of select="AccountNo"/>							
						</xsl:otherwise>
						
					</xsl:choose>
					</SECACCOUNT>
					
					<TRADECURRID>
						<xsl:value-of select="'USD'"/>
					</TRADECURRID>

					<SETLCURRID>
						<xsl:value-of select="'USD'"/>
					</SETLCURRID>

					<BSIND>
						<xsl:value-of select="substring(Side,1,1)"/>
					</BSIND>

					<INSTTYP>
						<!--<xsl:choose>
							<xsl:when test="TaxLotState = 'Allocated'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Y'"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="'N'"/>
					</INSTTYP>

					<PRICE>
						<xsl:choose>
							<xsl:when test="number($AvgofPrice)">
								<xsl:value-of select="format-number($AvgofPrice,'#.#####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>						
					</PRICE>

					<COMMISSION>
						<xsl:choose>
							<xsl:when test ="CurrencySymbol='USD'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number(CommissionCharged)">
										<xsl:value-of select="CommissionCharged"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						
					</COMMISSION>

					<STAMPTAX>
						<xsl:value-of select="''"/>
					</STAMPTAX>

					<LOCALCHGS>
						<xsl:value-of select="''"/>
					</LOCALCHGS>

					<INTEREST>
						<xsl:value-of select="''"/>
					</INTEREST>

					<PRINCIPAL>
						<xsl:value-of select="''"/>
					</PRINCIPAL>

					<SECFEE>
						<xsl:value-of select="''"/>
					</SECFEE>

					<EXECBROKER>
						<xsl:value-of select="''"/>
					</EXECBROKER>

					<BROKEROS>
						<xsl:value-of select="''"/>
					</BROKEROS>

					<TRAILERCD1>
						<xsl:value-of select="''"/>
					</TRAILERCD1>


					<TRAILERCD2>
						<xsl:value-of select="''"/>
					</TRAILERCD2>

					<TRAILERCD3>
						<xsl:value-of select="''"/>
					</TRAILERCD3>

					<BLOTTERCD>
						<xsl:value-of select="'80'"/>
					</BLOTTERCD>

					<CLRNGHSE>
						<xsl:value-of select="'Y'"/>
					</CLRNGHSE>

					<CLRAGNTCD>
						<xsl:choose>
							<xsl:when test="$varCounterParty = 'JPGA'">
								<xsl:value-of select="'JPMS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CLRAGNTCD>

					<CLRAGNT1>
						<xsl:value-of select="''"/>
					</CLRAGNT1>

					<CLRAGNT2>
						<xsl:value-of select="''"/>
					</CLRAGNT2>

					<CLRAGNT3>
						<xsl:value-of select="''"/>
					</CLRAGNT3>


					<CLRAGNT4>
						<xsl:value-of select="''"/>
					</CLRAGNT4>

					<CNTRPRTYCD>
						<xsl:value-of select="''"/>
					</CNTRPRTYCD>

					<CNTRPTY1>
						<xsl:value-of select="''"/>
					</CNTRPTY1>

					<CNTRPTY2>
						<xsl:value-of select="''"/>
					</CNTRPTY2>

					<CNTRPTY3>
						<xsl:value-of select="''"/>
					</CNTRPTY3>

					<CNTRPTY4>
						<xsl:value-of select="''"/>
					</CNTRPTY4>


					<INSTRUCT>
						<xsl:value-of select="''"/>
					</INSTRUCT>

					<CEDELAKV>
						<xsl:value-of select="''"/>
					</CEDELAKV>

					<ORIGLOCALREF>
						<xsl:value-of select="concat('T8A',substring(TradeDate,4,2),$HH,$SS,$AMPM)"/>
					</ORIGLOCALREF>

					
					<NOTES>
						<xsl:value-of select="concat('/EXECTIME=',$HH,$MM,$SS)"/>
					</NOTES>

					<FILLER1>
						<xsl:value-of select="''"/>
					</FILLER1>

					<FILLER2>
						<xsl:value-of select="''"/>
					</FILLER2>

					<RR>
						<xsl:value-of select="''"/>
					</RR>

					<SETLCOUNTRYCD>
						<xsl:value-of select="'US'"/>
					</SETLCOUNTRYCD>

					<INSTRUMENTTYPE>
						<xsl:value-of select="''"/>
					</INSTRUMENTTYPE>

					<COMMISSIONRATE>
						<xsl:value-of select="''"/>
					</COMMISSIONRATE>

					<COMPANYNO>
						<xsl:value-of select="''"/>
					</COMPANYNO>


					<Filler3>
						<xsl:value-of select="''"/>
					</Filler3>

					<Filler4>
						<xsl:value-of select="''"/>
					</Filler4>

					<Filler5>
						<xsl:value-of select="''"/>
					</Filler5>

					<Filler6>
						<xsl:value-of select="''"/>
					</Filler6>

					<Filler7>
						<xsl:value-of select="''"/>
					</Filler7>

					<GPF2IDCode>
						<xsl:value-of select="''"/>
					</GPF2IDCode>


					<GPF2Amount>
						<xsl:value-of select="''"/>
					</GPF2Amount>

					<GPF2CurrencyCode>
						<xsl:value-of select="''"/>
					</GPF2CurrencyCode>

					<GPF2AddSubtract>
						<xsl:value-of select="''"/>
					</GPF2AddSubtract>

					<GPF3IDCode>
						<xsl:value-of select="''"/>
					</GPF3IDCode>

					<GPF3Amount>
						<xsl:value-of select="''"/>
					</GPF3Amount>

					<GPF3CurrencyCode>
						<xsl:value-of select="''"/>
					</GPF3CurrencyCode>

					<GPF3AddSubtract>
						<xsl:value-of select="''"/>
					</GPF3AddSubtract>

					<GPF4IDCode>
						<xsl:value-of select="''"/>
					</GPF4IDCode>

					<GPF4Amount>
						<xsl:value-of select="''"/>
					</GPF4Amount>

					<GPF4CurrencyCode>
						<xsl:value-of select="''"/>
					</GPF4CurrencyCode>

					<GPF4AddSubtract>
						<xsl:value-of select="''"/>
					</GPF4AddSubtract>

					<GPF5IDCode>
						<xsl:value-of select="''"/>
					</GPF5IDCode>

					<GPF5Amount>
						<xsl:value-of select="''"/>
					</GPF5Amount>

					<GPF5CurrencyCode>
						<xsl:value-of select="''"/>
					</GPF5CurrencyCode>

					<GPF5AddSubtract>
						<xsl:value-of select="''"/>
					</GPF5AddSubtract>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
