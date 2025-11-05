<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime firstWednesday= new DateTime(year, month, 1);
		while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
		{
		firstWednesday = firstWednesday.AddDays(1);
		}
		return firstWednesday.ToString();
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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>


	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=01">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
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
		<xsl:param name="Suffix"/>
		<xsl:if test="COL83='CALLL' or COL83='PUTL'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL25,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL17,'/'),'/')"/>
				
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL17),'EXP'),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(COL17,'/'),'/'),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(substring-after(COL25,' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(substring-after(COL25,' '),' '),2),'##.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
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

		</xsl:if>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL34"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity) ">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'MS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="PB_ROOT_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_YELLOW_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>


						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @YellowFlag = $PB_YELLOW_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name ="FUTURE_EXCHANGE_CODE">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable name="Underlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="translate($PRANA_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate($PB_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="PRANA_Strike_Multiplier">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@UnderlyingCode = $Underlying]/@StrikeMul"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL83='CALLL' or COL83='PUTL'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL83='FUTURE'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Asset>
							<xsl:value-of select="$Asset"/>
						</Asset>

						<xsl:variable name="Symbol">
							<xsl:value-of select="COL25"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL25"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
								
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>



						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL6)"/>
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
									<xsl:value-of select="$Quantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>


						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL49"/>
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

						<xsl:variable name="Side" select="COL13"/>
						<Side>

							<xsl:choose>
								<xsl:when test="$Side='B'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$Side='S'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>



						<TradeDate>
							<xsl:value-of select="COL36"/>
						</TradeDate>


						<xsl:variable name="COL18">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL18"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL16">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>



						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL51"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="COL13">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL23"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL12">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="TaxOnCommissions">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL52"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFees>

							<xsl:choose>
								<xsl:when test="$TaxOnCommissions &gt; 0">
									<xsl:value-of select="$TaxOnCommissions"/>
								</xsl:when>

								<xsl:when test="$TaxOnCommissions &lt; 0">
									<xsl:value-of select="$TaxOnCommissions * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SecFees>

						<xsl:variable name="OrfFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL56"/>
							</xsl:call-template>
						</xsl:variable>
						<OrfFee>

							<xsl:choose>
								<xsl:when test="$OrfFee &gt; 0">
									<xsl:value-of select="$OrfFee"/>
								</xsl:when>

								<xsl:when test="$OrfFee &lt; 0">
									<xsl:value-of select="$OrfFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</OrfFee>

						<CurrencySymbol>
							<xsl:value-of select="COL21"/>
						</CurrencySymbol>

						<ExpirationDate>
							<xsl:value-of select="COL86"/>
						</ExpirationDate>>
								
					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>