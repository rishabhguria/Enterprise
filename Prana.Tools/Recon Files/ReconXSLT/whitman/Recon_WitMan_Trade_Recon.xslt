<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08 ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="MonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=01">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=02">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=03">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=04">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=05">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=06">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=07">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=08">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=09">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="COL12='OPTION' and COL34='INDEX'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL14,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL14,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL14),' '),' '),' '),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL14),'/'),'/'),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL14),' '),' '),' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(normalize-space(COL14),' '),' '),'00.00')"/>
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

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL23"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Witman'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

					

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>	
								<xsl:when test="contains(COL19,'PUT') or contains(COL19,'CALL')">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="'Future'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Asset>
							<xsl:value-of select="$Asset"/>
						</Asset>


						<xsl:variable name ="varFuture">
							<xsl:choose>
								<xsl:when test="$Asset='Future'">
									<xsl:choose>
										<xsl:when test="string-length(substring-before(COL16,' '))=5">
											<xsl:value-of select="concat(substring(substring-before(COL16,' '),1,3),' ',substring(substring-before(COL16,' '),4))"/>
										</xsl:when>
										<xsl:when test="string-length(substring-before(COL16,' '))=4">
											<xsl:value-of select="concat(substring(substring-before(COL16,' '),1,2),' ',substring(substring-before(COL16,' '),3))"/>
										</xsl:when>

										<xsl:when test="string-length(substring-before(COL16,' '))=1">
											<xsl:value-of select="concat(substring-before(COL16,' '),' ',substring-before(substring-after(COL16,' '),' '))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="COL16"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="varFutureOption">
							<xsl:choose>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:choose>
										<xsl:when test="string-length(substring-before(COL16,' '))=5">
											<xsl:value-of select="concat(substring(substring-before(COL16,' '),1,2),' ',substring(substring-before(COL16,' '),3),substring-before(substring-after(COL16,' '),' '))"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="COL16"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>



						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>			

			
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="$varFuture"/>
								</xsl:when>

								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="$varFutureOption"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						

						<xsl:variable name="PB_FUND_NAME" select="COL7"/>

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
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>


						<xsl:variable name="Side">
							<xsl:value-of select="COL22"/>
						</xsl:variable>
						<Side>
									<xsl:choose>
										<xsl:when  test="$Side='Buy'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when  test="$Side='Sell'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
						</Side>

						<xsl:variable name="TradePrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select=" COL24"/>
							</xsl:call-template>
						</xsl:variable>


						<AvgPX>
							<xsl:choose>
								<xsl:when test="$TradePrice &gt; 0">
									<xsl:value-of select="$TradePrice"/>
								</xsl:when>
								<xsl:when test="$TradePrice &lt; 0">
									<xsl:value-of select="$TradePrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL26"/>
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
						<xsl:variable name="COL27">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL27"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL28">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL29">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL30">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL48">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL48"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL49">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL49"/>
							</xsl:call-template>
						</xsl:variable>	

						
												
									
						<xsl:variable name="varAuecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$COL27 + $COL28 + $COL29 + $COL30 + $COL48 + $COL49"/>
							</xsl:call-template>
						</xsl:variable>
						<AUECFee1>
							<xsl:choose>
								<xsl:when test="$varAuecFee &gt; 0">
									<xsl:value-of select="$varAuecFee"/>
								</xsl:when>
								<xsl:when test="$varAuecFee &lt; 0">
									<xsl:value-of select="$varAuecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AUECFee1>

						<xsl:variable name="varOtherBroker">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL50"/>
							</xsl:call-template>
						</xsl:variable>
						<OtherBrokerFees>
							<xsl:choose>
								<xsl:when test="$varAuecFee &gt; 0">
									<xsl:value-of select="$varAuecFee"/>
								</xsl:when>
								<xsl:when test="$varAuecFee &lt; 0">
									<xsl:value-of select="$varAuecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OtherBrokerFees>



						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Date" select="COL1"/>
						<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>