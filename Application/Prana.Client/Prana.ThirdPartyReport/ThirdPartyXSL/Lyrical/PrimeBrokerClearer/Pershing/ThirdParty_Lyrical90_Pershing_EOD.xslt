<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="MonthNo"/>
		<xsl:choose>
			<xsl:when test="$MonthNo=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$MonthNo=12">
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

				<FileFooter>
					<xsl:value-of select="'true'"/>
				</FileFooter>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<LOCALREF>
					<xsl:value-of select="'LOCALREF'"/>
				</LOCALREF>

				<CFID>
					<xsl:value-of select="'CFID'"/>
				</CFID>

				<ROUTECD>
					<xsl:value-of select="'ROUTECD'"/>
				</ROUTECD>

				<TIRORDERID>
					<xsl:value-of select="'TIRORDERID'"/>
				</TIRORDERID>

				<TIRPIECE>
					<xsl:value-of select="'TIRPIECE'"/>
				</TIRPIECE>

				<TIRSEQ>
					<xsl:value-of select="'TIRSEQ'"/>
				</TIRSEQ>

				<SECIDTYPE>
					<xsl:value-of select="'SECIDTYPE'"/>
				</SECIDTYPE>

				<SECURITYID>
					<xsl:value-of select="'SECURITYID'"/>
				</SECURITYID>

				<DESCRIPTION1>
					<xsl:value-of select="'DESCRIPTION1'"/>
				</DESCRIPTION1>

				<DESCRIPTION2>
					<xsl:value-of select="'DESCRIPTION2'"/>
				</DESCRIPTION2>

				<DESCRIPTION3>
					<xsl:value-of select="'DESCRIPTION3'"/>
				</DESCRIPTION3>

				<DESCRIPTION4>
					<xsl:value-of select="'DESCRIPTION4'"/>
				</DESCRIPTION4>

				<TRADEDATE>
					<xsl:value-of select="'TRADEDATE'"/>
				</TRADEDATE>

				<SETLDATE>
					<xsl:value-of select="'SETLDATE'"/>
				</SETLDATE>

				<QUANTITY>
					<xsl:value-of select="'QUANTITY'"/>
				</QUANTITY>

				<QUANTITYDESC>
					<xsl:value-of select="'QUANTITYDESC'"/>
				</QUANTITYDESC>

				<NETMONEY>
					<xsl:value-of select="'NETMONEY'"/>
				</NETMONEY>

				<CASHACCOUNT>
					<xsl:value-of select="'CASHACCOUNT'"/>
				</CASHACCOUNT>

				<SECACCOUNT>
					<xsl:value-of select="'SECACCOUNT'"/>
				</SECACCOUNT>

				<TRADECURRID>
					<xsl:value-of select="'TRADECURRID'"/>
				</TRADECURRID>

				<SETLCURRID>
					<xsl:value-of select="'SETLCURRID'"/>
				</SETLCURRID>

				<BSIND>
					<xsl:value-of select="'BSIND'"/>
				</BSIND>

				<INSTTYP>
					<xsl:value-of select="'INSTTYP'"/>
				</INSTTYP>

				<PRICE>
					<xsl:value-of select="'PRICE'"/>
				</PRICE>

				<COMMISSION>
					<xsl:value-of select="'COMMISSION'"/>
				</COMMISSION>

				<STAMPTAX>
					<xsl:value-of select="'STAMPTAX'"/>
				</STAMPTAX>

				<LOCALCHGS>
					<xsl:value-of select="'LOCALCHGS'"/>
				</LOCALCHGS>

				<INTEREST>
					<xsl:value-of select="'INTEREST'"/>
				</INTEREST>

				<PRINCIPAL>
					<xsl:value-of select="'PRINCIPAL'"/>
				</PRINCIPAL>

				<SECFEE>
					<xsl:value-of select="'SECFEE'"/>
				</SECFEE>

				<EXECBROKER>
					<xsl:value-of select="'EXECBROKER'"/>
				</EXECBROKER>

				<BROKEROS>
					<xsl:value-of select="'BROKEROS'"/>
				</BROKEROS>

				<TRAILERCD1>
					<xsl:value-of select="'TRAILERCD1'"/>
				</TRAILERCD1>

				<TRAILERCD2>
					<xsl:value-of select="'TRAILERCD2'"/>
				</TRAILERCD2>

				<TRAILERCD3>
					<xsl:value-of select="'TRAILERCD3'"/>
				</TRAILERCD3>

				<BLOTTERCD>
					<xsl:value-of select="'BLOTTERCD'"/>
				</BLOTTERCD>

				<CLRNGHSE>
					<xsl:value-of select="'CLRNGHSE'"/>
				</CLRNGHSE>

				<CLRAGNTCD>
					<xsl:value-of select="'CLRAGNTCD'"/>
				</CLRAGNTCD>

				<CLRAGNT1>
					<xsl:value-of select="'CLRAGNT1'"/>
				</CLRAGNT1>

				<CLRAGNT2>
					<xsl:value-of select="'CLRAGNT2'"/>
				</CLRAGNT2>

				<CLRAGNT3>
					<xsl:value-of select="'CLRAGNT3'"/>
				</CLRAGNT3>

				<CLRAGNT4>
					<xsl:value-of select="'CLRAGNT4'"/>
				</CLRAGNT4>

				<CNTRPRTYCD>
					<xsl:value-of select="'CNTRPRTYCD'"/>
				</CNTRPRTYCD>

				<CNTRPTY1>
					<xsl:value-of select="'CNTRPTY1'"/>
				</CNTRPTY1>

				<CNTRPTY2>
					<xsl:value-of select="'CNTRPTY2'"/>
				</CNTRPTY2>

				<CNTRPTY3>
					<xsl:value-of select="'CNTRPTY3'"/>
				</CNTRPTY3>

				<CNTRPTY4>
					<xsl:value-of select="'CNTRPTY4'"/>
				</CNTRPTY4>

				<INSTRUCT>
					<xsl:value-of select="'INSTRUCT'"/>
				</INSTRUCT>

				<CEDELAKV>
					<xsl:value-of select="'CEDELAKV'"/>
				</CEDELAKV>

				<ORIGLOCALREF>
					<xsl:value-of select="'ORIGLOCALREF'"/>
				</ORIGLOCALREF>

				<NOTES>
					<xsl:value-of select="'NOTES'"/>
				</NOTES>

				<FILLER1>
					<xsl:value-of select="'FILLER1'"/>
				</FILLER1>

				<FILLER2>
					<xsl:value-of select="'FILLER2'"/>
				</FILLER2>

				<RR>
					<xsl:value-of select="'RR'"/>
				</RR>

				<SETLCOUNTRYCD>
					<xsl:value-of select="'SETLCOUNTRYCD'"/>
				</SETLCOUNTRYCD>

				<INSTRUMENTTYPE>
					<xsl:value-of select="'INSTRUMENTTYPE'"/>
				</INSTRUMENTTYPE>

				<COMMISSIONRATE>
					<xsl:value-of select="'COMMISSIONRATE'"/>
				</COMMISSIONRATE>

				<COMPANYNO>
					<xsl:value-of select="'COMPANYNO'"/>
				</COMPANYNO>

				<Filler3>
					<xsl:value-of select="'Filler3'"/>
				</Filler3>

				<Filler4>
					<xsl:value-of select="'Filler4'"/>
				</Filler4>

				<Filler5>
					<xsl:value-of select="'Filler5'"/>
				</Filler5>

				<Filler6>
					<xsl:value-of select="'Filler6'"/>
				</Filler6>

				<Filler7>
					<xsl:value-of select="'Filler7'"/>
				</Filler7>

				<GPF2IDCode>
					<xsl:value-of select="'GPF2 ID Code'"/>
				</GPF2IDCode>

				<GPF2Amount>
					<xsl:value-of select="'GPF2 Amount'"/>
				</GPF2Amount>

				<GPF2CurrencyCode>
					<xsl:value-of select="'GPF2 Currency Code'"/>
				</GPF2CurrencyCode>

				<GPF2AddSubtract>
					<xsl:value-of select="'GPF2 Add Subtract'"/>
				</GPF2AddSubtract>

				<GPF3IDCode>
					<xsl:value-of select="'GPF3 ID Code'"/>
				</GPF3IDCode>

				<GPF3Amount>
					<xsl:value-of select="'GPF3 Amount'"/>
				</GPF3Amount>

				<GPF3CurrencyCode>
					<xsl:value-of select="'GPF3 Currency Code'"/>
				</GPF3CurrencyCode>

				<GPF3AddSubtract>
					<xsl:value-of select="'GPF3 Add Subtract'"/>
				</GPF3AddSubtract>

				<GPF4IDCode>
					<xsl:value-of select="'GPF4 ID Code'"/>
				</GPF4IDCode>

				<GPF4Amount>
					<xsl:value-of select="'GPF4 Amount'"/>
				</GPF4Amount>

				<GPF4CurrencyCode>
					<xsl:value-of select="'GPF4 Currency Code'"/>
				</GPF4CurrencyCode>

				<GPF4AddSubtract>
					<xsl:value-of select="'GPF4 Add Subtract'"/>
				</GPF4AddSubtract>

				<GPF5IDCode>
					<xsl:value-of select="'GPF5 ID Code'"/>
				</GPF5IDCode>'

				<GPF5Amount>
					<xsl:value-of select="'GPF5 Amount'"/>
				</GPF5Amount>

				<GPF5CurrencyCode>
					<xsl:value-of select="'GPF5 Currency Code'"/>
				</GPF5CurrencyCode>

				<GPF5AddSubtract>
					<xsl:value-of select="'GPF5 Add Subtract'"/>
				</GPF5AddSubtract>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>
			
			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'true'"/>
					</FileFooter>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<!--for system internal use-->
					<LOCALREF>
						<xsl:value-of select="'LYR14335'"/>
					</LOCALREF>

					<CFID>
						<xsl:value-of select="'LYR14335'"/>
					</CFID>

					<ROUTECD>
						<xsl:value-of select="'PSHG'"/>
					</ROUTECD>

					<TIRORDERID>
						<xsl:value-of select="EntityID"/>
					</TIRORDERID>

					<TIRPIECE>
						<xsl:value-of select="''"/>
					</TIRPIECE>

					<TIRSEQ>
						<xsl:value-of select="''"/>
					</TIRSEQ>

					<SECIDTYPE>
						<xsl:value-of select="'C'"/>
					</SECIDTYPE>

					<SECURITYID>
						<xsl:value-of select="CUSIP"/>
					</SECURITYID>

					<DESCRIPTION1>
						<xsl:value-of select="FullSecurityName"/>
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

					<xsl:variable name="MonthNoTr" select="substring-before(TradeDate,'/')"/>

					<xsl:variable name="MonthNameTr">
						<xsl:call-template name="MonthName">
							<xsl:with-param name="MonthNo" select="$MonthNoTr"/>
						</xsl:call-template>
					</xsl:variable>
					
					<TRADEDATE>
						<xsl:value-of select="concat(substring-before(substring-after(TradeDate,'/'),'/'),'-',$MonthNameTr,'-',substring-after(substring-after(TradeDate,'/'),'/'))"/>
					</TRADEDATE>

					<xsl:variable name="MonthNoSe" select="substring-before(SettlementDate,'/')"/>

					<xsl:variable name="MonthNameSe">
						<xsl:call-template name="MonthName">
							<xsl:with-param name="MonthNo" select="$MonthNoSe"/>
						</xsl:call-template>
					</xsl:variable>

					<SETLDATE>
						<xsl:value-of select="concat(substring-before(substring-after(SettlementDate,'/'),'/'),'-',$MonthNameSe,'-',substring-after(substring-after(SettlementDate,'/'),'/'))"/>
					</SETLDATE>

					<QUANTITY>
						<xsl:value-of select="AllocatedQty"/>
					</QUANTITY>

					<QUANTITYDESC>
						<xsl:value-of select="''"/>
					</QUANTITYDESC>

					<NETMONEY>
						<xsl:value-of select="''"/>
					</NETMONEY>

					<CASHACCOUNT>
						<xsl:choose>
							<xsl:when test="AccountNo='Roger C. Altman: QXV002556'">
								<xsl:value-of select="'QXV0025562'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountNo"/>
							</xsl:otherwise>
						</xsl:choose>
						
						
						
						
						<!--<xsl:value-of select="concat(FundAccountNo,2)"/>-->
					</CASHACCOUNT>

					<SECACCOUNT>
						<xsl:choose>
							<xsl:when test="AccountNo='Roger C. Altman: QXV002556'">
								<xsl:value-of select="'QXV0025562'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountNo"/>
							</xsl:otherwise>
						</xsl:choose>
					</SECACCOUNT>

					<TRADECURRID>
						<xsl:choose>
							<xsl:when test="CurrencySymbol!=''">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'USD'"/>
							</xsl:otherwise>
						</xsl:choose>
					</TRADECURRID>

					<SETLCURRID>
						<xsl:choose>
							<xsl:when test="CurrencySymbol!=''">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'USD'"/>
							</xsl:otherwise>
						</xsl:choose>
					</SETLCURRID>

					<BSIND>
						<xsl:choose>
							<xsl:when test="contains(Side,'Buy')">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BSIND>

					<INSTTYP>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated' or TaxLotState='Amended'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'Y'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</INSTTYP>

					<PRICE>
						<xsl:value-of select="format-number(AveragePrice,'0.######')"/>
					</PRICE>

					<COMMISSION>
						<xsl:value-of select="''"/>
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
						<xsl:value-of select="''"/>
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
						<xsl:value-of select="'PAS14335'"/>
					</ORIGLOCALREF>

					<NOTES>
						<xsl:value-of select="''"/>
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
					</GPF5IDCode>'

					<GPF5Amount>
						<xsl:value-of select="''"/>
					</GPF5Amount>

					<GPF5CurrencyCode>
						<xsl:value-of select="''"/>
					</GPF5CurrencyCode>

					<GPF5AddSubtract>
						<xsl:value-of select="''"/>
					</GPF5AddSubtract>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>