<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}

	</msxsl:script>

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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB' ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR' ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='APR' ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY' ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN' ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP' ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT' ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV' ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC' ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB' ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR' ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='APR' ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY' ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN' ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP' ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT' ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV' ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC' ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>

		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="normalize-space(COL10)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring(substring-after(normalize-space(COL11),' '),5,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring(substring-after(normalize-space(COL11),' '),1,3)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after(normalize-space(COL11),' '),10,2)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring-before(normalize-space(substring(substring-after(normalize-space(COL11),' '),12)),' '),'##.00')"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(COL11,string-length(COL11),1)"/>
		</xsl:variable>

		<xsl:variable name="MonthCodeVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="($ExpiryMonth)"/>
				<xsl:with-param name="PutOrCall" select="$PutORCall"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="substring($ExpiryDay,1,1)='0'">
					<xsl:value-of select="substring($ExpiryDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ExpiryDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
	</xsl:template>
	
	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL6)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)and (COL2='Buy' or COL2='Cover Buy' or COL2='Sell' or COL2='Short Sell')">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>					
						
						<xsl:variable name="varAsset">
							<xsl:value-of select="normalize-space(COL26)"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>

						<xsl:variable name="AssetType">
							<xsl:choose>
								<xsl:when test="contains($varAsset,'OPTION')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$AssetType='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space(COL11)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
												
						<xsl:variable name="PB_FUND_NAME" select="COL1"/>
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>											

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>
								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>
						
												
						<xsl:variable name="TotalClr" select="COL47"/>						
												
						<xsl:variable name="ExchangeFee" select="COL68"/>
																			
						<xsl:variable name="RegulatoryFees" select="COL78"/>
						
						
						<xsl:variable name="AddingColums">
							<xsl:choose>
								<xsl:when test="$ExchangeFee=0 and $RegulatoryFees=0">
									<xsl:value-of select="$TotalClr"/>
								</xsl:when>	
								<xsl:when test="$ExchangeFee=0 and number($RegulatoryFees)">
									<xsl:value-of select="$TotalClr + $RegulatoryFees"/>
								</xsl:when>
								<xsl:when test="$RegulatoryFees=0 and number($ExchangeFee)">
									<xsl:value-of select="$TotalClr + $ExchangeFee"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$TotalClr + $ExchangeFee + $RegulatoryFees"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varTotalCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$AddingColums"/>
							</xsl:call-template>
						</xsl:variable>
						
						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$varTotalCommission &gt; 0">
									<xsl:value-of select="$varTotalCommission"/>
								</xsl:when>
								<xsl:when test="$varTotalCommission &lt; 0">
									<xsl:value-of select="$varTotalCommission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommissionandFees>

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
							</xsl:call-template>
						</xsl:variable>
											
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

						<TradeDate>
							<xsl:value-of select ="COL3"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select ="COL4"/>
						</SettlementDate>

						<xsl:variable name="Side">
							<xsl:value-of select="COL24"/>
						</xsl:variable>
												
						<Side>
							<xsl:choose>
								<xsl:when test ="$AssetType='Option'">
									<xsl:choose>								
										<xsl:when test="$Side='BUY' and COL25='TO OPEN'">
											<xsl:value-of select="'Buy to open'"/>
										</xsl:when>
										<xsl:when test="$Side='BUY' and COL25='TO CLOSE'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:when test="$Side='SELL' and COL25='TO OPEN'">
											<xsl:value-of select="'Sell to open'"/>
										</xsl:when>
										<xsl:when test="$Side='SELL' and COL25='TO CLOSE'">
											<xsl:value-of select="'Sell to close'"/>
										</xsl:when>								
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$Side='SELL'">
											<xsl:value-of select ="'Sell'"/>
										</xsl:when>
										<xsl:when test ="$Side='BUY'">
											<xsl:value-of select ="'Buy'"/>
										</xsl:when>
										<xsl:when test ="$Side='SHORT SELL'">
											<xsl:value-of select ="'Sell short'"/>
										</xsl:when>
										<xsl:when test ="$Side='COVER BUY'">
											<xsl:value-of select ="'Buy to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
								        </xsl:otherwise>
							        </xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


