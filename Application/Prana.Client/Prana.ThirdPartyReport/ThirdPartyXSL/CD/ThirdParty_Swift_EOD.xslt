<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	
	<xsl:template name="GetBICCode">
		<xsl:param name="CountryName"/>
		<xsl:param name="CounterParty"/>
		<xsl:choose>
			<xsl:when test="$CounterParty = 'WCHV'">
				<xsl:choose>
					<xsl:when test="$CountryName = 'China'">
						<xsl:value-of select="'HSBCHKHHSEC'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Belgium'">
						<xsl:value-of select="'PARBFRPP'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Portugal'">
						<xsl:value-of select="'PARBFRPP'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Netherlands'">
						<xsl:value-of select="'PARBFRPP'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'NewZealand'">
						<xsl:value-of select="'NATANZ22'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'SouthAfrica'">
						<xsl:value-of select="'SBZAZAJJ'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Denmark'">
						<xsl:value-of select="'ESSEDKKK'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Spain'">
						<xsl:value-of select="'SABNESMMSSS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Germany'">
						<xsl:value-of select="'PARBDEFF'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Australia'">
						<xsl:value-of select="'CHASAU2XCCS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Finland'">
						<xsl:value-of select="'ESSEFIHX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Mexico'">
						<xsl:value-of select="'CITIUS33MER'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Italy'">
						<xsl:value-of select="'PARBITMM'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Japan'">
						<xsl:value-of select="'MHCBJPJ2'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Norway'">
						<xsl:value-of select="'ESSENOKX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'France'">
						<xsl:value-of select="'PARBFRPP'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Canada'">
						<xsl:value-of select="'ROYCCAT2SET'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Singapore'">
						<xsl:value-of select="'DEUTSGSGCUS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Switzerland'">
						<xsl:value-of select="'UBSWCHZH80A'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Sweden'">
						<xsl:value-of select="'ESSESESS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Argentina'">
						<xsl:value-of select="'CITIUS33ARR'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'United States'">
						<xsl:value-of select="'PNBPUS3CLBR'"/>
					</xsl:when>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$CountryName = 'Argentina'">
						<xsl:value-of select="'BSSCHARBASSS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Austria'">
						<xsl:value-of select="'DEUTATWWCUS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Australia'">
						<xsl:value-of select="'CITIAU3X'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Belgium'">
						<xsl:value-of select="'CITTGB2L'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Brazil'">
						<xsl:value-of select="'BNIFBRRJ'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Canada'">
						<xsl:value-of select="'CITICATT'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Denmark'">
						<xsl:value-of select="'NDEADKKK'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Finland'">
						<xsl:value-of select="'NDEAFIHH'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'France'">
						<xsl:value-of select="'CITTGB2L'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Germany'">
						<xsl:value-of select="'DEUTDEFFCUS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Greece'">
						<xsl:value-of select="'CITIGRAA'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'HongKong'">
						<xsl:value-of select="'SCBLHKHH'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Hungary'">
						<xsl:value-of select="'INGBHUHB'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Iceland'">
						<xsl:value-of select="'ARIOISRE'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Indonesia'">
						<xsl:value-of select="'SCBLIDJX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Israel'">
						<xsl:value-of select="'LUMIILIT'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Italy'">
						<xsl:value-of select="'CITIITMX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Japan'">
						<xsl:value-of select="'CITIJPJT'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Malaysia'">
						<xsl:value-of select="'SCBLMYKX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Mexico'">
						<xsl:value-of select="'CITIUS33MER'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Netherlands'">
						<xsl:value-of select="'CITTGB2L'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'NewZealand'">
						<xsl:value-of select="'CITINZ2X'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Norway'">
						<xsl:value-of select="'NDEANOKK'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Philippines'">
						<xsl:value-of select="'SCBLPHMM'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Poland'">
						<xsl:value-of select="'INGBPLPW'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Portugal'">
						<xsl:value-of select="'CITIPTPX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Singapore'">
						<xsl:value-of select="'SCBLSGSG'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'SouthAfrica'">
						<xsl:value-of select="'FIRNZAJJ'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'SouthKorea'">
						<xsl:value-of select="'SCBLKRSE'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Spain'">
						<xsl:value-of select="'PARBESMX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Sweden'">
						<xsl:value-of select="'NDEASESS'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Switzerland'">
						<xsl:value-of select="'CITICHZZ'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'Thailand'">
						<xsl:value-of select="'SCBLTHBX'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'United Kingdom'">
						<xsl:value-of select="'CRST/393'"/>
					</xsl:when>
					<xsl:when test="$CountryName = 'United States'">
						<xsl:value-of select="'JEFFUS33'"/>
					</xsl:when>
				</xsl:choose>

			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetCodeCounterParty">
		<xsl:param name="CounterParty">
			<xsl:choose>
				<xsl:when test="CounterParty = 'JEFF'">
					<xsl:value-of select="'JEFFUS33'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:param>
	</xsl:template>


	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

					<!--for system internal use-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'false'"/>
					</IsCaptionChangeRequired>

					<FileHeader>
						<xsl:value-of select ="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select ="'true'"/>
					</FileFooter>

					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varMessageType">
						<xsl:choose>
							<xsl:when test="substring(Side,1,1) = 'B'">
								<xsl:value-of select="541"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="543"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varTradeHour">
						<xsl:value-of select="substring-before(substring-after(TradeDateTime, ' '),':')"/>
					</xsl:variable>

					<xsl:variable name="varTradeTime">
						<xsl:choose>
							<xsl:when test="string-length($varTradeHour) = 1">
								<xsl:value-of select="concat('0', $varTradeHour, substring(substring-after(TradeDateTime, ':'), 1, 2))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat($varTradeHour, substring(substring-after(TradeDateTime, ':'), 1, 2))"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varTradeDate">
						<xsl:value-of select="concat(substring(TradeDate, 9, 2), substring(TradeDate, 1, 2), substring(TradeDate, 4, 2))"/>
					</xsl:variable>

					<xsl:variable name ="varCountry">
						<xsl:value-of select ="Country"/>
					</xsl:variable>

					<xsl:variable name ="varBICCode">
						<xsl:value-of select="document('../ReconMappingXml/BICCodeMapping.xml')/BICCodeMapping/PB[@Name= 'GS']/BICData[@CountryName=$varCountry]/@BICCode"/>
					</xsl:variable>

					<xsl:variable name="varSenderBICCode">
						<xsl:call-template name="GetBICCode">
							<xsl:with-param name="CountryName" select="Country"/>
							<xsl:with-param name="CounterParty" select="CounterParty"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varSettleDate">
						<xsl:value-of select="concat(substring(SettlementDate, 7, 4), substring(SettlementDate, 1, 2), substring(SettlementDate, 4, 2))"/>
					</xsl:variable>

					<xsl:variable name="varFundID">
						<xsl:choose>
							<xsl:when test="CompanyAccountID = 7">
								<xsl:value-of select="1"/>
							</xsl:when>
							
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varZeros">
						<xsl:call-template name="noofzeros">
							<xsl:with-param name="count" select="(10-string-length(PBUniqueID)- string-length($varFundID))"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varUniqueID">
						<xsl:value-of select="concat($varZeros, PBUniqueID, $varFundID)"/>
					</xsl:variable>


					<Block1>
						<xsl:value-of select="concat('{1:F01IRVTUS3NASTC', $varUniqueID,'}','{2:O', $varMessageType, $varTradeTime, $varTradeDate, 'MAERISLANDXX', $varUniqueID, $varTradeDate, $varTradeTime, 'N}{4:')"/>
					</Block1>

						<!--<xsl:value-of select="concat('{2:O', $varMessageType, $varTradeTime, $varTradeDate, $varSenderBICCode, PBUniqueID, $varTradeDate, $varTradeTime, 'N}{4:')"/>-->

					<Seq_A1>
						<xsl:text>&#xD;</xsl:text>
						<xsl:value-of select="':16R:GENL'"/>
					</Seq_A1>

          
					<Seq_A2>
						<xsl:text>&#xD;</xsl:text>
						<xsl:value-of select="concat(':20C','::SEME//4274700000')"/>
					</Seq_A2>

          
					<Seq_A3>
						<xsl:text>&#xD;</xsl:text>
						<xsl:value-of select="':23G:NEWM'"/>
					</Seq_A3>

          
					<Seq_A4>
						<xsl:text>&#xD;</xsl:text>
						<xsl:value-of select="':16S:GENL'"/>
					</Seq_A4>

          
					<Seq_A5>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:TRADDET'"/>
					</Seq_A5>

          
					<Seq_A6>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':98A::SETT//', $varSettleDate)"/>
					</Seq_A6>

          
					<Seq_A7>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':98A::TRAD//', '20', $varTradeDate)"/>
					</Seq_A7>

          
					<Seq_A8>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':90A::DEAL//PRCT/', AveragePrice)"/>
					</Seq_A8>


					<xsl:variable name="varSecurity">
						<xsl:choose>
							<xsl:when test="ISIN != ''">
								<xsl:value-of select="concat(substring(ISIN,1,2),'/',substring(ISIN,3))"/>
							</xsl:when>
							<xsl:when test="CUSIP != ''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<Seq_B1>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':35B: ISIN ', ISIN)"/>
					</Seq_B1>
					
          
					<Seq_B2>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="FullSecurityName"/>
					</Seq_B2>

          
					<Seq_B3>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:TRADDET'"/>
					</Seq_B3>

          
					<Seq_B4>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:FIAC'"/>
					</Seq_B4>

          
					<Seq_B5>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':36B::SETT//UNIT/', AllocatedQty, ',')"/>
					</Seq_B5>

					<xsl:variable name="varGrossNotionalBase">
						<xsl:choose>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="GrossAmount*ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="GrossAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varCommissionFee">
								<xsl:value-of select="format-number(CommissionCharged + OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees,'#.00')"/>
					</xsl:variable>

					<xsl:variable name="varNetNotionalBase">
						<xsl:choose>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="NetAmount*ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="NetAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Seq_E1>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':97A::SAFE//', '999999')"/>
					</Seq_E1>

          
					<Seq_E2>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:FIAC'"/>
					</Seq_E2>

          
					<Seq_E3>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:SETDET'"/>
					</Seq_E3>

          
					<Seq_E4>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':22F::SETR//TRAD'"/>
					</Seq_E4>

          
					<Seq_E5>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:SETPRTY'"/>
					</Seq_E5>

          
					<Seq_E6>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':95P::PSET//', $varBICCode)"/>
					</Seq_E6>

          
					<Seq_E7>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:SETPRTY'"/>
					</Seq_E7>

          
					<Seq_E8>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:SETPRTY'"/>
					</Seq_E8>

					<xsl:variable name="varAgentType">
						<xsl:choose>
							<xsl:when test="substring(Side, 1, 1) = 'B'">
								<xsl:value-of select="'DEAG'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'REAG'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varCode">
						<xsl:call-template name="GetCodeCounterParty">
							<xsl:with-param name="CounterParty" select="CounterParty"/>
						</xsl:call-template>
					</xsl:variable>
					
					<Seq_E9>
						<xsl:text>&#xa;</xsl:text>
						<xsl:choose>
							<xsl:when test ="contains(Symbol, '-LON') != false">
								<xsl:value-of select ="concat(':95R::', $varAgentType ,'//', 'CRST/393')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(':95R::', $varAgentType ,'//', 'JEFFUS33')"/>
							</xsl:otherwise>
						</xsl:choose>
					</Seq_E9>

          
					<Seq_E10>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:SETPRTY'"/>
					</Seq_E10>

          
					<Seq_E11>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:SETPRTY'"/>
					</Seq_E11>

					<xsl:variable name="varReverseSide">
						<xsl:choose>
							<xsl:when test="substring(Side, 1, 1) = 'B'">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'BUYR'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<Seq_E12>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':95R::', $varReverseSide, '//', $varSenderBICCode)"/>
					</Seq_E12>

          
					<Seq_E13>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:SETPRTY'"/>
					</Seq_E13>

          
					<Seq_E14>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:AMT'"/>
					</Seq_E14>

          <xsl:variable name="varGrossAmount">
            <xsl:value-of select="format-number(GrossAmount, '#.00')"/>
          </xsl:variable>

					<Seq_E15>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':19A::DEAL//', CurrencySymbol, substring-before($varGrossAmount,'.'),substring-after($varGrossAmount, '.'))"/>
					</Seq_E15>

          
					<Seq_E16>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:AMT'"/>
					</Seq_E16>

          
					<Seq_E17>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:AMT'"/>
					</Seq_E17>

          
					<Seq_E18>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':19A::ACRU//', CurrencySymbol, substring-before($varCommissionFee,'.'),'.',substring-after($varCommissionFee,'.'))"/>
					</Seq_E18>


					<Seq_E19>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:AMT'"/>
					</Seq_E19>


					<Seq_E20>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16R:AMT'"/>
					</Seq_E20>

          <xsl:variable name="varNetAmount">
            <xsl:value-of select="format-number(NetAmount,'#.00')"/>
          </xsl:variable>

					<Seq_E21>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="concat(':19A::SETT//',CurrencySymbol, substring-before($varNetAmount,'.'), substring-after($varNetAmount,'.'))"/>
					</Seq_E21>

          
					<Seq_E22>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:AMT'"/>
					</Seq_E22>

          
					<Seq_E23>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="':16S:SETDET'"/>
					</Seq_E23>

          
					<Seq_E24>
						<xsl:text>&#xa;</xsl:text>
						<xsl:value-of select="'-}'"/>
					</Seq_E24>

          
          <ALLOCQTY>
            <xsl:value-of select="AllocatedQty"/>
          </ALLOCQTY>

          <InternalNetNotional>
            <xsl:value-of select="NetAmount"/>
          </InternalNetNotional>

          <InternalGrossAmount>
            <xsl:value-of select="GrossAmount"/>
          </InternalGrossAmount>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
