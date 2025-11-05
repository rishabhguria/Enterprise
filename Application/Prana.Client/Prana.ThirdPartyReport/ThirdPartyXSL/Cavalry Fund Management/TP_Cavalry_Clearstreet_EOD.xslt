<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail [CounterParty != 'CLST']">
        <ThirdPartyFlatFileDetail>
        
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
         
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <type>
            <xsl:value-of select="'away_trade'"/>
          </type>


          <client_trade_id>
            <xsl:value-of select="EntityID"/>
          </client_trade_id>
		  
		    <xsl:variable name="varTimeStamp">
           <xsl:value-of select="translate(substring-before(substring-after(substring-after(substring-after(TradeDateTime,'/'),'/'),' '),' '),':','')"/>
          </xsl:variable>

          <timestamp>
             <!-- <xsl:choose> -->
              <!-- <xsl:when test="contains(TradeDateTime,'PM')"> -->
			   <!-- <xsl:choose> -->
              <!-- <xsl:when test="string-length($varTimeStamp) = 5"> -->
                <!-- <xsl:value-of select="concat(substring($varTimeStamp,1,1)+12, substring($varTimeStamp,2))"/> -->
              <!-- </xsl:when> -->
                <!-- <xsl:otherwise> -->
               <!-- <xsl:value-of select="concat(substring($varTimeStamp,1,2)+12, substring($varTimeStamp,3))"/> -->
              <!-- </xsl:otherwise> -->
            <!-- </xsl:choose> -->
              <!-- </xsl:when> -->
             
              <!-- <xsl:otherwise> -->
               <!-- <xsl:value-of select="translate(substring-before(substring-after(substring-after(substring-after(TradeDateTime,'/'),'/'),' '),' '),':','')"/> -->
              <!-- </xsl:otherwise> -->
            <!-- </xsl:choose> -->
			<xsl:value-of select="''"/>
			
          </timestamp>
		  
		  <date>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </date>

          <account_id>
            <xsl:value-of select="AccountNo"/>
          </account_id>

     
          <quantity>
            <xsl:value-of select="AllocatedQty"/>
          </quantity>

          <price>
            <xsl:value-of select="AveragePrice"/>
          </price>

          <instrument.identifier>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </instrument.identifier>

          <instrument.identifier_type>
            <xsl:value-of select="'ticker'"/>
          </instrument.identifier_type>

          <instrument.country>
            <xsl:choose>
              <xsl:when test="CurrencySymbol='USD'">
                <xsl:value-of select="'USA'"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </instrument.country>

          <instrument.currency>
            <xsl:value-of select="CurrencySymbol"/>
          </instrument.currency>

          <side.direction>
            <xsl:choose>
              <xsl:when test="contains(Side,'Buy')">
                <xsl:value-of select="'buy'"/>
              </xsl:when>
              <xsl:when test="contains(Side,'Sell')">
                <xsl:value-of select="'sell'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </side.direction>

          <capacity>
            <xsl:value-of select="'agency'"/>
          </capacity>

          <fees.commission>
            <xsl:value-of select ="SoftCommissionCharged + CommissionCharged"/>
          </fees.commission>
		  
		  
		<xsl:variable name="varCounterParty" select="CounterParty"/>
		
		
		<xsl:variable name="varExecMPIDCode">
            <xsl:value-of select="document('../ReconMappingXml/TP_BrokerEXECMPIDMapping.xml')/BrokerMapping/PB[@Name='CLST']/BrokerData[@PranaBroker=$varCounterParty]/@EXECMPIDCode"/>
          </xsl:variable>

          <xsl:variable name="varMPID">
            <xsl:choose>
              <xsl:when test="$varExecMPIDCode!=''">
                <xsl:value-of select="$varExecMPIDCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <!-- <exec_mpid> -->
            <!-- <xsl:value-of select="CounterParty"/> -->
          <!-- </exec_mpid> -->
		  
		  <exec_mpid>
           <xsl:value-of select="$varMPID"/> 
          </exec_mpid>
		  
		  
		  <xsl:variable name="varCONTRAMPIDCode">
            <xsl:value-of select="document('../ReconMappingXml/TP_BrokerCONTRAMPIDMapping.xml')/BrokerMapping/PB[@Name='CLST']/BrokerData[@PranaBroker=$varCounterParty]/@CONTRAMPIDCode"/>
          </xsl:variable>

          <xsl:variable name="varCONTRAMPIDCode_Value">
            <xsl:choose>
              <xsl:when test="$varCONTRAMPIDCode!=''">
                <xsl:value-of select="$varCONTRAMPIDCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <!-- <contra_mpid> -->
            <!-- <xsl:value-of select="CounterParty"/> -->
          <!-- </contra_mpid> -->
		  
		  <contra_mpid>
            <xsl:value-of select="$varCONTRAMPIDCode_Value"/>
          </contra_mpid>

          
          <xsl:variable name="THIRDPARTY_DTC_NAME">
            <xsl:value-of select="document('../ReconMappingXml/TP_BrokerCONTRAClearingMapping.xml')/BrokerMapping/PB[@Name='CLST']/BrokerData[@PranaBroker=$varCounterParty]/@DTCCode"/>
          </xsl:variable>

          <xsl:variable name="varDTC">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_DTC_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_DTC_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
		  
          <contra_clearing_num>
            <xsl:value-of select="$varDTC"/>
          </contra_clearing_num>

          <side.position>
            <xsl:choose>
              <xsl:when test="contains(Asset,'EquityOption')">
                <xsl:choose>
                  <xsl:when test="contains(Side,'Close')">
                    <xsl:value-of select="'close'"/>
                  </xsl:when>
                  <xsl:when test="contains(Side,'Open')">
                    <xsl:value-of select="'open'"/>
                  </xsl:when>
                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </side.position>

          <side.qualifier>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:choose>
                  <xsl:when test="contains(Side,'short')">
                    <xsl:value-of select="'short'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </side.qualifier>



          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
