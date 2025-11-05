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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="MonthColumn">
		<xsl:param name="MonthSett"/>
		<xsl:choose>
			<xsl:when test="$MonthSett='Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Feb' ">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Mar' ">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Apr' ">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='May' ">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Jun' ">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Jul'  ">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Aug'  ">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Sep' ">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Oct' ">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Nov' ">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$MonthSett='Dec' ">
				<xsl:value-of select="'12'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(COL2),' ')"/>
		</xsl:variable>

		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(normalize-space(COL2),'/'),'/')"/>
		</xsl:variable>

		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL2),' '),' '),'/')"/>
		</xsl:variable>

		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL2),'/'),'/'),' ')"/>
		</xsl:variable>

		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(substring-after(substring-after(COL2,'/'),'/'),' '),1,1)"/>
		</xsl:variable>		

		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL2),'/'),'/'),' '),' '),2),'##.00')"/>
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

			<xsl:for-each select ="//PositionMaster">								

				<xsl:variable name="varAssetType">
					<xsl:choose>
						<xsl:when test="substring(substring-after(substring-after(substring-after(COL2,'/'),'/'),' '),1,1) = 'P'  or substring(substring-after(substring-after(substring-after(COL2,'/'),'/'),' '),1,1) = 'C'">
							<xsl:value-of select="'EquityOption'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Equity'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
				 <xsl:variable name="varMarkPrice">
				 	<xsl:choose>
				 		<xsl:when test="$varAssetType = 'EquityOption'">
				 			<xsl:value-of select="normalize-space(COL3)"/>
				 		</xsl:when>
				 		<xsl:when test="$varAssetType = 'Equity'">
				 			<xsl:value-of select="normalize-space(COL4)"/>
				 		</xsl:when>				 		
				 		<xsl:otherwise>
				 			<xsl:value-of select="0"/>
				 		</xsl:otherwise>
				 	</xsl:choose>
				 </xsl:variable>				
				
				<xsl:if test="number($varMarkPrice) and  not(contains(COL2,'CURNCY'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>
						
						<xsl:variable name="varBloomberg">
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>						

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAssetType='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space($varSymbol)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$varBloomberg != ''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSymbol != ''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select="''"/>
								</xsl:when>							
								<xsl:when test="$varBloomberg != ''">
									<xsl:value-of select="$varBloomberg"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>
						
						<xsl:variable name = "PB_FUND_NAME">
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
					
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPrice &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="$varMarkPrice &lt; 0">
									<xsl:value-of select="$varMarkPrice * (1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>					

						<Date>
							<xsl:value-of select ="COL1"/>
						</Date>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>